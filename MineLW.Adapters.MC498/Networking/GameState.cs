using MineLW.Adapters.MC498.Networking.Client;
using MineLW.Adapters.MC498.Networking.Server;
using MineLW.API.Text;
using MineLW.Networking;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking
{
    public class GameState : NetworkState
    {
        public override IMessage CreateDisconnectMessage(TextComponent reason)
        {
            return new MessageClientDisconnect.Message(reason);
        }

        protected override IMessageSerializer[] GetSerializers()
        {
            return new IMessageSerializer[]
            {
                null,
                null,
                null,
                null,
                null,
                new MessageClientSpawnPlayer(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                new MessageClientChatMessage(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                new MessageClientCustomData(),
                null,
                new MessageClientDisconnect(),
                null,
                null,
                new MessageClientUnloadChunk(),
                null,
                null,
                new MessageClientPingRequest(),
                new MessageClientLoadChunk(),
                null,
                null,
                null,
                new MessageClientInitGame(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                new MessageClientPlayerInfo(),
                new MessageClientRotateToward(),
                new MessageClientPlayerTeleport(),
                null,
                new MessageClientDestroyEntities(),
                null,
                null,
                new MessageClientRespawn(),
                null,
                null,
                null,
                null,
                null,
                new MessageClientUpdateViewPosition(),
            };
        }

        protected override IMessageDeserializer[] GetDeserializers()
        {
            return new IMessageDeserializer[]
            {
                new MessageServerPlayerTeleportConfirm(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                new MessageServerPingResponse(), 
                null,
                new MessageServerPlayerPosition(),
                new MessageServerPlayerPositionRotation(), 
                new MessageServerPlayerRotation(),
                null,
            };
        }

        public override MessageController CreateController(NetworkClient client)
        {
            return new ClientController(client);
        }
    }
}