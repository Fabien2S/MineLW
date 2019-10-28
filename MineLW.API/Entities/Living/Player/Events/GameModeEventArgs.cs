using System;

namespace MineLW.API.Entities.Living.Player.Events
{
    public class GameModeEventArgs : EventArgs
    {
        public readonly GameMode From;
        public readonly GameMode To;

        public GameModeEventArgs(GameMode from, GameMode to)
        {
            From = from;
            To = to;
        }
    }
}