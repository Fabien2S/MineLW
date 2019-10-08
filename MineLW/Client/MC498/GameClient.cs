using System.Numerics;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.Client.MC498.Client;
using MineLW.Entities.Living.Player;
using MineLW.Networking;

namespace MineLW.Client.MC498
{
    public class GameClient : MineLW.Client.GameClient
    {
        public GameClient(NetworkClient networkClient) : base(networkClient)
        {
            var entityPlayer = new EntityPlayer(this);
        }

        protected internal override void InitGame(IEntityPlayer player)
        {
            NetworkClient.Send(new MessageClientInitGame.Message(
                player.Id,
                0,
                0,
                0,
                0,
                "default",
                8,
                false,
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