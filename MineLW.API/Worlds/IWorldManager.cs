using System;
using MineLW.API.Blocks;
using MineLW.API.Registries;
using MineLW.API.Utils;
using MineLW.API.Worlds.Events;

namespace MineLW.API.Worlds
{
    public interface IWorldManager
    {
        /// <summary>
        /// The world name where the player will spawn on log in.
        /// </summary>
        Identifier DefaultWorld { get; }
        
        IBlockRegistry BlockRegistry { get; }

        event EventHandler<WorldEventArgs> WorldCreated;

        /// <summary>
        /// Create a new world.
        /// </summary>
        /// <remarks>If the name already exists, the matching world is returned</remarks>
        /// <param name="name">The name for the world</param>
        /// <returns>The world with the given name</returns>
        IWorld CreateWorld(Identifier name);

        /// <summary>
        /// Request an unique id across all the worlds
        /// </summary>
        /// <returns>an unique id</returns>
        int RequestUniqueId();

        IWorld this[Identifier name] { get; }
    }
}