using MineLW.Adapters;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.Networking;
using MineLW.Networking.Messages;
using MineLW.Protocols.Status.Client;

namespace MineLW.Protocols.Status
{
    public class StatusController : MessageController
    {
        public StatusController(NetworkClient client) : base(client)
        {
        }

        public void HandleInfoRequest()
        {
            var version = GameAdapter.IsSupported(Client.Version.Protocol) ? Client.Version : GameAdapter.ServerVersion;
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

            Client.Send(new MessageClientServerInfo.Message(status));
        }

        public void HandlePing(in long payload)
        {
            Client
                .Send(new MessageClientPong.Message(payload))
                .ContinueWith(task => Client.Close("Pong sent"));
        }
    }
}