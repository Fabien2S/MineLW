using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.Networking.Messages;
using MineLW.Networking.States.Status.Client;

namespace MineLW.Networking.States.Status
{
    public class StatusController : MessageController
    {
        public StatusController(NetworkClient client) : base(client)
        {
        }

        public void HandleInfoRequest()
        {
            Client.Send(new MessageClientServerInfo.Message(
                new Version("MineLW v0.1a", 5),
                0, 20, new PlayerProfile[0], new TextComponentString("Hi")
                {
                    Color = TextColor.Black
                }, null
            ));
        }

        public void HandlePing(in long payload)
        {
            Client.Send(new MessageClientPong.Message(payload), task =>
            {
                Client.Disconnect("Pong sent");
            });
        }
    }
}