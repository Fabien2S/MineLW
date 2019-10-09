using MineLW.API.Worlds;

namespace MineLW.API
{
    public interface IServer
    {
        IWorldManager WorldManager { get; }
        
        void Start();
        void Shutdown();
    }
}