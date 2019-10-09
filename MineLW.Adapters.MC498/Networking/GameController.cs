using MineLW.Networking;
using MineLW.Networking.Messages;

namespace MineLW.Adapters.MC498.Networking
{
    public class GameController : MessageController
    {
        public GameController(NetworkClient client) : base(client)
        {
        }
    }
}