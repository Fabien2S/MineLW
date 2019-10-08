using MineLW.Networking;
using MineLW.Networking.Messages;

namespace MineLW.Client.MC498
{
    public class GameController : MessageController
    {
        public GameController(NetworkClient client) : base(client)
        {
        }
    }
}