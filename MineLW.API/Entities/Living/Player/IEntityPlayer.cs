using System;
using MineLW.API.Entities.Living.Player.Events;

namespace MineLW.API.Entities.Living.Player
{
    public interface IEntityPlayer : IEntityLiving
    {
        PlayerProfile Profile { get; }
        
        PlayerMode PlayerMode { get; set; }
        
        event EventHandler<PlayerGameModeEventArgs> GameModeChanged;
    }
}