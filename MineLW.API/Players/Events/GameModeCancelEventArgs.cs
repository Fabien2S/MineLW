using System.ComponentModel;

namespace MineLW.API.Players.Events
{
    public class GameModeCancelEventArgs : CancelEventArgs
    {
        public readonly GameMode From;
        public readonly GameMode To;

        public GameModeCancelEventArgs(GameMode from, GameMode to)
        {
            From = from;
            To = to;
        }
    }
}