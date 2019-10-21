using MineLW.API.Blocks;
using MineLW.API.Client;
using MineLW.API.Worlds;

namespace MineLW.API
{
    public interface IServer
    {
        string Name { get; }

        IBlockManager BlockManager { get; }

        IWorldManager WorldManager { get; }
        IClientManager ClientManager { get; }

        void Start();
        void Shutdown();
    }
}