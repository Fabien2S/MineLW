using MineLW.API.Client;
using MineLW.API.Commands;
using MineLW.API.Worlds;

namespace MineLW.API.Server
{
    public interface IServer
    {
        string Name { get; }

        IWorldManager WorldManager { get; }
        IClientManager ClientManager { get; }
        ICommandManager CommandManager { get; }

        void Start();
        void Shutdown();
    }
}