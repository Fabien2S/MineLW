using MineLW.Adapter.MC498.Networking.Client;
using MineLW.Adapter.MC498.Networking.Networking.Client;
using MineLW.API.Text;
using MineLW.Networking;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapter.MC498.Networking
{
    public class GameState498 : NetworkState
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
                null,
                null,
                new MessageClientDisconnect(),
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
                new MessageClientPlayerTeleport()
            };
        }

        protected override IMessageDeserializer[] GetDeserializers()
        {
            return new IMessageDeserializer[0];
        }

        public override MessageController CreateController(NetworkClient client)
        {
            return new GameController498(client);
        }
    }
}