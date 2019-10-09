using MineLW.API.Utils;

namespace MineLW.API.Worlds
{
    public interface IWorldManager
    {
        /// <summary>
        /// The world name where the player will spawn on log in.
        /// </summary>
        Identifier DefaultWorld { get; set; }

        /// <summary>
        /// Create a new world.
        /// </summary>
        /// <remarks>If the name already exists, the matching world is returned</remarks>
        /// <param name="name">The name for the world</param>
        /// <returns>The world with the given name</returns>
        IWorld CreateWorld(Identifier name);
    }
}