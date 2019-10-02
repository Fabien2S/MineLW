using System;
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

        public bool IsConnected => _channel != null && _channel.Open;

        public NetworkAdapter Adapter;
        
        private IChannel _channel;
        private bool _closed;

        public override void ChannelActive(IChannelHandlerContext context)
        {
            _channel = context.Channel;
            _channel.Configuration.AutoRead = true;
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            if (!IsConnected)
                return;

            Logger.Error("Failed to send message: {0}", exception);
            Close(exception.Message);
        }

        public void Update(float deltaTime)
        {
        }

        private void Close(string reason)
        {
            if (_closed || !IsConnected)
                return;

            _closed = true;
            Logger.Info("Closing connection {0}: {1}", this, reason);
            _channel.CloseAsync();
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, IMessage msg)
        {
        }

        public override string ToString()
        {
            return _channel.RemoteAddress.ToString();
        }
    }
}