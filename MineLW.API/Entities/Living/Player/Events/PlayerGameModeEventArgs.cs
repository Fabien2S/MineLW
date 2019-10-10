using MineLW.API.Utils;

namespace MineLW.API.Entities.Living.Player.Events
{
    public class PlayerGameModeEventArgs : PlayerEventArgs, ICancellable
    {
        public bool Canceled { get; set; }

        public readonly GameMode From;
        public readonly GameMode To;

        public PlayerGameModeEventArgs(IEntityPlayer entity, GameMode from, GameMode to) : base(entity)
        {
            From = from;
            To = to;
        }
    }
}