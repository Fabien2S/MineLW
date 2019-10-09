using MineLW.Adapter;
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
            var version = GameAdapter.IsSupported(Client.Version) ? Client.Version : GameAdapter.Default;
            var status = new ServerStatus(
                version,
                new PlayerInfo(
                    0, 20, new PlayerProfile[0]
                ),
                new TextComponentString("Minecraft ")
                {
                    Color = TextColor.Blue,
                    Children =
                    {
                        new TextComponentString("#" + version.Protocol)
                        {
                            Color = TextColor.Gold,
                            Style = TextStyles.None
                        },
                        new TextComponentString("\nThe server is now live!")
                        {
                            Style = TextStyles.Italic | TextStyles.Underlined
                        }
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