using MineLW.API.Utils;
using MineLW.API.Worlds;

namespace MineLW.API.Client.Events
{
    public class ClientWorldContextEventArgs : ClientEventArgs, ICancellable
    {
        public bool Cancelled { get; set; }
        
        public readonly IWorldContext WorldContext;
        
        public ClientWorldContextEventArgs(IClient client, IWorldContext worldContext) : base(client)
        {
            WorldContext = worldContext;
        }
    }
}