using MineLW.API.Text;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;
using MineLW.Networking.States.Status.Client;
using MineLW.Networking.States.Status.Server;

namespace MineLW.Networking.States.Status
{
    public class StatusState : NetworkState
    {
        public static readonly StatusState Instance = new StatusState();
        
        private StatusState() : base(true)
        {
        }

        public override IMessage CreateDisconnectMessage(TextComponent reason)
        {
            return null;
        }

        protected override IMessageSerializer[] GetSerializers()
        {
            return new IMessageSerializer[]
            {
                new MessageClientServerInfo(),
                new MessageClientPong()
            };
        }

        protected override IMessageDeserializer[] GetDeserializers()
        {
            return new IMessageDeserializer[]
            {
                new MessageServerRequestInfo(),
                new MessageServerPing()
            };
        }

        protected internal override MessageController CreateController(NetworkClient client)
        {
            return new StatusController(client);
        }
    }
}