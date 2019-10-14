using MineLW.API.Utils;

namespace MineLW.API.Worlds
{
    public interface IWorld : IWorldContext
    {
        IWorldContext CreateContext(Identifier name);
    }
}