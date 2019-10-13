using MineLW.API.Client.World;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Utils;

namespace MineLW.API.Client
{
    public interface IClient : IUpdatable
    {
        IClientConnection Connection { get; }
        PlayerProfile Profile { get; }

        IEntityPlayer Player { get; }
        IClientWorld World { get; }

        void Init(IEntityPlayer player);
    }
}