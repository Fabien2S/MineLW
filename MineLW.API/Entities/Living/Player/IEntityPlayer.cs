using System;
using MineLW.API.Entities.Living.Player.Events;
using MineLW.API.Utils;

namespace MineLW.API.Entities.Living.Player
{
    public interface IEntityPlayer : IEntityLiving
    {
        PlayerProfile Profile { get; }
        
        GameMode GameMode { get; set; }
        
        event EventHandler<PlayerGameModeEventArgs> GameModeChanged;
    }
}