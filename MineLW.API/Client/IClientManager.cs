using MineLW.API.Entities.Living.Player;
using MineLW.API.Utils;

namespace MineLW.API.Client
{
    public interface IClientManager : IUpdatable
    {
        void Initialize(IClientConnection connection, PlayerProfile profile);
    }
}