using MineLW.Adapters.MC498.Networking.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.API.Worlds;
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
            var worldContext = player.WorldContext;
            var environment = worldContext.GetOption(WorldOption.Environment);
            
            NetworkClient.Send(new MessageClientInitGame.Message(
                player.Id,
                (byte) player.GameMode,
                environment.Id,
                0,
                "default",
                8,
                false
            ));
            NetworkClient.Send(new MessageClientPlayerTeleport.Message(
                player.Position,
                player.Rotation,
                0
            ));
        }

        public override void SendMessage(TextComponent message)
        {
            NetworkClient.Send(new MessageClientChatMessage.Message(message));
        }

        public override void Disconnect(TextComponentString reason)
        {
            NetworkClient.Send(new MessageClientDisconnect.Message(reason));
        }
    }
}