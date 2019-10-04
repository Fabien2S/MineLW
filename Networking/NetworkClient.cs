using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using MineLW.API.Utils;
using MineLW.Debugging;
using MineLW.Networking.Messages;

namespace MineLW.Networking
{
    public class NetworkClient : SimpleChannelInboundHandler<IMessage>, IUpdatable
    {
        public const string Name = "message_handler";

        private static readonly Logger Logger = LogManager.GetLogger<NetworkClient>();

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

        private readonly ConcurrentQueue<IMessage> _receivedMessages = new ConcurrentQueue<IMessage>();
        private readonly ConcurrentQueue<QueuedMessage> _queuedMessages = new ConcurrentQueue<QueuedMessage>();

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
            Disconnect("End of stream");
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Logger.Error("Network exception: {0}", exception);
            Disconnect(exception.Message);
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, IMessage msg)
        {
            if (_state.Async)
                _state.Handle(_controller, msg);
            else
                _receivedMessages.Enqueue(msg);
        }

        public void Update(float deltaTime)
        {
            while (_receivedMessages.TryDequeue(out var message))
                _state.Handle(_controller, message);

            while (_queuedMessages.TryDequeue(out var message))
            {
                var task = _channel.WriteAndFlushAsync(message.Message);
                if (message.Callback != null)
                    task.ContinueWith(message.Callback);
            }
        }

        public void Send(IMessage message, Action<Task> callback = null)
        {
            if (_state.Async)
            {
                var task = _channel.WriteAndFlushAsync(message);
                if (callback != null)
                    task.ContinueWith(callback);
            }
            else
                _queuedMessages.Enqueue(new QueuedMessage(message, callback));
        }

        public void Disconnect(string reason)
        {
            if (Closed)
                return;

            Logger.Info("Closing connection {0} (reason: {1})", this, reason);

            Closed = true;
            _channel?.CloseAsync();
            _channel = null;
        }

        public override string ToString()
        {
            return _channel.RemoteAddress.ToString();
        }

        private struct QueuedMessage
        {
            public readonly IMessage Message;
            public readonly Action<Task> Callback;

            public QueuedMessage(IMessage message, Action<Task> callback)
            {
                Message = message;
                Callback = callback;
            }
        }
    }
}