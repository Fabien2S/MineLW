using MineLW.API.Text;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;
using MineLW.Networking.States.Login.Client;
using MineLW.Networking.States.Login.Server;

namespace MineLW.Networking.States.Login
{
    public class LoginState : NetworkState
    {
        public static readonly LoginState Instance = new LoginState();
        
        private LoginState() : base(true)
        {
        }

        public override IMessage CreateDisconnectMessage(TextComponent reason)
        {
            return new MessageClientDisconnect.Message(reason);
        }

        protected override IMessageSerializer[] GetSerializers()
        {
            return new IMessageSerializer[] {
                new MessageClientDisconnect(),
                new MessageClientEncryptionRequest(),
                new MessageClientLoginResponse(),
                new MessageClientEnableCompression()
            };
        }

        protected override IMessageDeserializer[] GetDeserializers()
        {
            return new IMessageDeserializer[]
            {
                new MessageServerLoginRequest(),
                new MessageServerEncryptionResponse()
            };
        }

        protected internal override MessageController CreateController(NetworkClient client)
        {
            return new LoginController(client);
        }
    }
}