using DotNetty.Buffers;
using MineLW.Adapters.MC498.Networking.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.API.Worlds;
using MineLW.Networking;

namespace MineLW.Adapters.MC498.Networking
{
    public class GameClient : Adapters.Networking.GameClient
    {
        private const string LevelType = "default";
        
        public GameClient(PlayerProfile profile, NetworkClient client) : base(client, profile)
        {
        }

        protected override void _Init(IEntityPlayer player)
        {
            var worldContext = player.WorldContext;
            var environment = worldContext.GetOption(WorldOption.Environment);
            
            NetworkClient.Send(new MessageClientInitGame.Message(
                player.Id,
                (byte) player.PlayerMode,
                environment.Id,
                0,
                LevelType,
                World.RenderDistance,
                false
            ));
            NetworkClient.Send(new MessageClientPlayerTeleport.Message(
                player.Position,
                player.Rotation,
                0
            ));
        }

        public override void SendCustom(Identifier channel, IByteBuffer buffer)
        {
            NetworkClient.Send(new MessageClientCustomData.Message(channel, buffer));
        }

        public override void SendMessage(TextComponent message)
        {
            NetworkClient.Send(new MessageClientChatMessage.Message(message));
        }

        public override void Disconnect(TextComponentString reason)
        {
            NetworkClient.Send(new MessageClientDisconnect.Message(reason));
        }

        public override void Respawn(IWorldContext worldContext)
        {
            var environment = worldContext.GetOption(WorldOption.Environment);
            NetworkClient.Send(new MessageClientRespawn.Message(
                environment.Id,
                Player.PlayerMode,
                LevelType
            ));
        }
    }
}