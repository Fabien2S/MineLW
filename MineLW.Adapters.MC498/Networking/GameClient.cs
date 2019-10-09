using MineLW.Adapters.MC498.Networking.Client;
using MineLW.Adapters.MC498.Networking.Networking.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.Networking;

namespace MineLW.Adapters.MC498.Networking
{
    public class GameClient : Adapters.GameClient
    {
        public GameClient(PlayerProfile profile, NetworkClient client) : base(profile, client)
        {
        }

        public override void Init(IEntityPlayer player)
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