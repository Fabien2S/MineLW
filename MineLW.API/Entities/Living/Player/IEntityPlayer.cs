using MineLW.API.Utils;

namespace MineLW.API.Entities.Living.Player
{
    public interface IEntityPlayer : IEntityLiving
    {
        PlayerProfile Profile { get; }
    }
}