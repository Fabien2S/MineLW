using MineLW.Adapters.MC498.Networking.Client;
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
                null,
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
                null,
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
                null,
                null,
                new MessageClientPlayerTeleport(),
                null,
                null,
                null,
                null,
                new MessageClientRespawn()
            };
        }

        protected override IMessageDeserializer[] GetDeserializers()
        {
            return new IMessageDeserializer[0];
        }

        public override MessageController CreateController(NetworkClient client)
        {
            return new GameController(client);
        }
    }
}