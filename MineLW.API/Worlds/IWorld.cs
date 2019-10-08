using MineLW.API.Utils;
using MineLW.API.Worlds.Context;

namespace MineLW.API.Worlds
{
    public interface IWorld : IWorldContext
    {
        IWorldContext CreateContext(Identifier name);
    }
}