using System;
using MineLW.API.Utils;
using MineLW.Networking.Messages;
using MineLW.Networking.States.Login;
using MineLW.Networking.States.Status;

namespace MineLW.Networking.States.Handshake
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