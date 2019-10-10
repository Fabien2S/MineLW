using System;

namespace MineLW.API.Entities.Living.Player.Events
{
    public class PlayerEventArgs : EventArgs
    {
        public readonly IEntityPlayer Player;

        public PlayerEventArgs(IEntityPlayer entity)
        {
            Player = entity;
        }
    }
}