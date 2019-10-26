using MineLW.Adapters;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.Networking;
using MineLW.Networking.Messages;
using MineLW.Protocols.Status.Client;

namespace MineLW.Protocols.Status
{
    public class StatusController : MessageController
    {
        public StatusController(NetworkClient networkClient) : base(networkClient)
        {
        }

        public void HandleInfoRequest()
        {
            var version = GameAdapters.IsSupported(NetworkClient.Version.Protocol) ? NetworkClient.Version : GameAdapters.CurrentVersion;
            var status = new ServerStatus(
                version,
                new PlayerInfo(
                    0, 20, new PlayerProfile[0]
                ),
                new TextComponentString("Using game adapter ")
                {
                    Color = TextColor.Green,
                    Children =
                    {
                        new TextComponentString(version.ToString())
                    }
                }
            );

            NetworkClient.Send(new MessageClientServerInfo.Message(status));
        }

        public void HandlePing(in long payload)
        {
            NetworkClient
                .Send(new MessageClientPong.Message(payload))
                .ContinueWith(task => NetworkClient.Close());
        }
    }
}