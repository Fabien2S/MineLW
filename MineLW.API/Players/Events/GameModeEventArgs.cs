using System;

namespace MineLW.API.Players.Events
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