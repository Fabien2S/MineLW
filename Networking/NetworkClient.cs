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

        public GameVersion Version { get; set; }
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
            Disconnect("End of stream");
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Logger.Error("Network exception: {0}", exception);
            Disconnect(exception.Message);
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
            while (_tasks.TryDequeue(out var task))
            {
                task.RunSynchronously();
                task.Wait();
            }
        }

        public Task Send(IMessage message)
        {
            if (_state.Async)
                return _channel.WriteAndFlushAsync(message);

            var task = new Task(msg => _channel.WriteAndFlushAsync(msg), message);
            _tasks.Enqueue(task);
            return task;
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
    }
}