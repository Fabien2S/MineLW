using MineLW.API.Client;
using MineLW.API.Text;
using MineLW.Client.MC498.Client;
using MineLW.Networking;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Client.MC498
{
    public class GameState : Networking.States.Game.GameState
    {
        public override IMessage CreateDisconnectMessage(TextComponent reason)
        {
            return null;
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

        protected override MessageController CreateController(NetworkClient client)
        {
            return new GameController(client);
        }
        
        public override IClient CreateClient(NetworkClient networkClient)
        {
            return new GameClient(networkClient);
        }
    }
}