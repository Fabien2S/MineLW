using System.Numerics;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.Client.MC498.Client;
using MineLW.Networking;

namespace MineLW.Client.MC498
{
    public class GameClient498 : GameClient
    {
        public GameClient498(NetworkClient networkClient) : base(networkClient)
        {
        }

        protected internal override void InitGame(IEntityPlayer player)
        {
            NetworkClient.Send(new MessageClientInitGame.Message(
                player.Id,
                0,
                0,
                0,
                "default",
                8,
                false
            ));
            NetworkClient.Send(new MessageClientPlayerTeleport.Message(
                Vector3.Zero, Vector2.Zero,0 
            ));
        }

        public override void SendMessage(TextComponent message)
        {
            NetworkClient.Send(new MessageClientChatMessage.Message(message));
        }

        public override void Kick(TextComponentString reason)
        {
            NetworkClient.Send(new MessageClientDisconnect.Message(reason));
        }
    }
}