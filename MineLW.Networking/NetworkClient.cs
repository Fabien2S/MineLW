using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using MineLW.API;
using MineLW.API.Server;
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

        public GameVersion Version { get; set; }
        public bool Closed { get; private set; }

        public NetworkState State
        {
            get => _state;
            set
            {
                _state = value;
                Controller = value.CreateController(this);
            }
        }

        public MessageController Controller { get; private set; }

        public readonly IServer Server;
        
        private readonly ConcurrentQueue<Task> _tasks = new ConcurrentQueue<Task>();

        private IChannel _channel;
        private NetworkState _state;

        public NetworkClient(IServer server)
        {
            Server = server;
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            _channel = context.Channel;
            _channel.Configuration.AutoRead = true;
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Close();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Logger.Error("An error occurred with {0}", this);
            Logger.Error(exception);
            Close();
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, IMessage message)
        {
            if (_state.Async)
            {
                _state.Handle(Controller, message);
                return;
            }

            var task = new Task(msg => _state.Handle(Controller, (IMessage) msg), message);
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
            catch(AggregateException e)
            {
#if DEBUG
                var innerException = e.InnerException;
                if (innerException == null)
                {
                    Disconnect();
                    return;
                }

                Logger.Error(innerException);
                var targetSite = innerException.TargetSite;
                Disconnect(new TextComponentTranslate("multiplayer.disconnect.generic")
                {
                    Color = TextColor.Red,
                    Style = TextStyles.Underlined,
                    Children =
                    {
                        new TextComponentString("\n"),
                        new TextComponentString(innerException.Message)
                        {
                            Style = TextStyles.None,
                            Children =
                            {
                                new TextComponentString("\nIn "),
                                new TextComponentString(targetSite.DeclaringType?.FullName + '#'),
                                new TextComponentString(targetSite.Name)
                            }
                        }
                    }
                });
#else
                Disconnect();
                return;
#endif
            }
        }

        public void EnableCompression(int threshold)
        {
            // TODO handle compression threshold update
            
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

        public void AddTask(Action task)
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
                Close();
                return;
            }

            Send(message).ContinueWith(task =>
            {
                var reason = reasonComponent.ToString();
                Logger.Info("{0} disconnected (reason: {1})", this, reason);
                Close();
            });
        }

        public void Close()
        {
            if (Closed)
                return;

            Closed = true;
            _channel?.CloseAsync();
            _channel = null;
        }

        public override string ToString()
        {
            return _channel.RemoteAddress.ToString();
        }
    }
}