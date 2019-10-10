using DotNetty.Buffers;
using MineLW.API.Client;
using MineLW.API.Worlds;

namespace MineLW.API
{
    public interface IServer
    {
        IWorldManager WorldManager { get; }
        IClientManager ClientManager { get; }
        string Name { get; }

        void Start();
        void Shutdown();
    }
}