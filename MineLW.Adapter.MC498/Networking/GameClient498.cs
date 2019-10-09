using MineLW.Adapter.MC498.Networking.Client;
using MineLW.Adapter.MC498.Networking.Networking.Client;
using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.Networking;

namespace MineLW.Adapter.MC498.Networking
{
    public class GameClient498 : IClient
    {
        public PlayerProfile Profile { get; }

        private readonly NetworkClient _client;

        public GameClient498(PlayerProfile profile, NetworkClient client)
        {
            Profile = profile;
            
            _client = client;
        }

        public void Init(IEntityPlayer player)
        {
            _client.Send(new MessageClientInitGame.Message(
                player.Id,
                0,
                0,
                0,
                "default",
                8,
                false
            ));
        }

        public void SendMessage(TextComponent message)
        {
            _client.Send(new MessageClientChatMessage.Message(message));
        }

        public void Kick(TextComponentString reason)
        {
            _client.Send(new MessageClientDisconnect.Message(reason));
        }
    }
}