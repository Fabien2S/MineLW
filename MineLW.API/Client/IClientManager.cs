using MineLW.API.Utils;

namespace MineLW.API.Client
{
    public interface IClientManager : IUpdatable
    {
        void Initialize(IClient client);
    }
}