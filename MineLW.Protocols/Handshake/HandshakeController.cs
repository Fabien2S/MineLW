using System;
using MineLW.API.Utils;
using MineLW.Networking;
using MineLW.Networking.Messages;
using MineLW.Protocols.Login;
using MineLW.Protocols.Status;

namespace MineLW.Protocols.Handshake
{
    public class HandshakeController : MessageController
    {
        public HandshakeController(NetworkClient client) : base(client)
        {
        }

        public void HandleHandshake(HandshakeMessage.Message message)
        {
            Client.Version = new GameVersion("Unknown", message.Protocol);
            Client.State = message.RequestedState switch
            {
                1 => (NetworkState) StatusState.Instance,
                2 => (NetworkState) LoginState.Instance,
                _ => throw new NotSupportedException("Invalid requested state: " + message.RequestedState)
            };
        }
    }
}