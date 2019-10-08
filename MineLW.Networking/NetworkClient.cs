using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.Networking.Handlers;
using MineLW.Networking.Messages;
using NLog;

namespace MineLW.Networking
{
    public class NetworkClient : SimpleChannelInboundHandler<IMessage>, IUpdatable
    {
        public const string Name = "message_handler";
        
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly TextComponent DefaultDisconnectReason = new TextComponentTranslate("multiplayer.disconnect.generic")
        {
            Color = TextColor.Red
        };

        public GameVersion Version { get; internal set; }
        public PlayerProfile Profile { get; internal set; }
        public bool Closed { get; private set; }

        public NetworkState State
        {
            get => _state;
            set
            {
                _state = value;
                _controller = value.CreateController(this);
            }
        }

        private readonly ConcurrentQueue<Task> _tasks = new ConcurrentQueue<Task>();

        private IChannel _channel;
        private NetworkState _state;
        private MessageController _controller;

        public override void ChannelActive(IChannelHandlerContext context)
        {
            _channel = context.Channel;
            _channel.Configuration.AutoRead = true;
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Close("End of stream");
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            #if DEBUG
            Logger.Error("An error occurred with {0}: {1}", this, exception);
            #endif
            Close(exception.Message);
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, IMessage message)
        {
            if (_state.Async)
            {
                _state.Handle(_controller, message);
                return;
            }

            var task = new Task(msg => _state.Handle(_controller, (IMessage) msg), message);
            _tasks.Enqueue(task);
        }

        public void Update(float deltaTime)
        {
            try
            {
                while (_tasks.TryDequeue(out var task))
                {
                    task.RunSynchronously();
                    task.Wait();
                }
            }
            catch
            {
                Disconnect();
            }
        }

        public void EnableCompression(int threshold)
        {
            Logger.Debug("Enabling compression on {0} (threshold: {1})", this, threshold);
            _channel.Pipeline.AddBefore(
                MessageEncodingHandler.Name,
                CompressionHandler.Name,
                new CompressionHandler(threshold)
            );
        }

        public void EnableEncryption(byte[] sharedSecret)
        {
            Logger.Debug("Enabling encryption on {0}", this);
            _channel.Pipeline.AddBefore(
                MessageFramingHandler.Name,
                EncryptionHandler.Name,
                new EncryptionHandler(sharedSecret)
            );
        }

        internal void AddTask(Action task)
        {
            _tasks.Enqueue(new Task(task));
        }

        public Task Send(IMessage message)
        {
            if (_state.Async)
                return _channel.WriteAndFlushAsync(message);

            var task = new Task(msg => _channel.WriteAndFlushAsync(msg), message);
            _tasks.Enqueue(task);
            return task;
        }

        public void Disconnect(TextComponent reasonComponent = null)
        {
            if(Closed)
                return;

            reasonComponent ??= DefaultDisconnectReason;
            
            var message = _state.CreateDisconnectMessage(reasonComponent);
            if (message == null)
            {
                Close(reasonComponent.ToString());
                return;
            }

            Send(message).ContinueWith(task =>
            {
                var reason = reasonComponent.ToString();
                Logger.Info("{0} disconnected (reason: {1})", this, reason);
                Close(reason);
            });
        }

        public void Close(string reason)
        {
            if (Closed)
                return;

            Logger.Debug("Closing connection {0} (reason: {1})", this, reason);

            Closed = true;
            _channel?.CloseAsync();
            _channel = null;
        }

        public override string ToString()
        {
            return Profile.Name + '[' + _channel.RemoteAddress + ']';
        }
    }
}