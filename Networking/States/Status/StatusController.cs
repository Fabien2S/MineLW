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
            var version = NetworkAdapter.IsSupported(Client.Version) ? Client.Version : NetworkAdapter.Default;
            var status = new ServerStatus
            {
                GameVersion = version,
                Description = new TextComponentString().WithValue("Hi!").WithColor(TextColor.Aqua)
            };

            var playerInfo = new PlayerInfo {Max = 20, Online = 10, Players = new PlayerProfile[0]};
            status.Players = playerInfo;

            Client.Send(new MessageClientServerInfo.Message(status));
        }

        public void HandlePing(in long payload)
        {
            Client
                .Send(new MessageClientPong.Message(payload))
                .ContinueWith(task => Client.Disconnect("Pong sent"));
        }
    }
}