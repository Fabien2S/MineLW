using System;
using MineLW.API.Client;
using MineLW.API.Entities.Living.Player.Events;

namespace MineLW.API.Entities.Living.Player
{
    public interface IEntityPlayer : IEntityLiving
    {
        /// <summary>
        /// Gets the player profile
        /// </summary>
        PlayerProfile Profile { get; }

        /// <summary>
        /// Determines if the current player is tied to a client
        /// </summary>
        bool HasClient { get; }

        /// <summary>
        /// Gets the client tied to the player, or null if none
        /// </summary>
        IClient Client { get; }

        /// <summary>
        /// <para>Gets or sets if the player is listed as an online player</para>
        /// This value is currently used to:
        /// <list type="bullet">
        /// <item><description>determines the player visibility inside the player list</description></item>  
        /// <item><description>determines if the player count toward the total players currently connected to the server</description></item>  
        /// </list>
        /// </summary>
        bool IsListed { get; set; }

        /// <summary>
        /// Gets or sets the player game mode
        /// </summary>
        GameMode GameMode { get; set; }

        event EventHandler<GameModeCancelEventArgs> GameModeChanging;
        event EventHandler<GameModeEventArgs> GameModeChanged;
    }
}