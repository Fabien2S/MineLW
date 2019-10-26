using System;
using MineLW.Adapters;
using MineLW.API.Utils;
using MineLW.Networking;
using MineLW.Networking.Messages;
using MineLW.Protocols.Login;
using MineLW.Protocols.Status;

namespace MineLW.Protocols.Handshake
{
    public class HandshakeController : MessageController
    {
        public HandshakeController(NetworkClient networkClient) : base(networkClient)
        {
        }

        public void HandleHandshake(HandshakeMessage.Message message)
        {
            NetworkClient.Version = GameAdapters.IsSupported(message.Protocol) ? GameAdapters.GetVersion(message.Protocol) : new GameVersion("Unknown", message.Protocol);
            NetworkClient.State = message.RequestedState switch
            {
                1 => (NetworkState) StatusState.Instance,
                2 => (NetworkState) LoginState.Instance,
                _ => throw new NotSupportedException("Invalid requested state: " + message.RequestedState)
            };
        }
    }
}