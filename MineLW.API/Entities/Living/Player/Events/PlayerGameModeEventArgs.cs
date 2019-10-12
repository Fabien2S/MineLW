using MineLW.API.Utils;

namespace MineLW.API.Entities.Living.Player.Events
{
    public class PlayerGameModeEventArgs : PlayerEventArgs, ICancellable
    {
        public bool Canceled { get; set; }

        public readonly PlayerMode From;
        public readonly PlayerMode To;

        public PlayerGameModeEventArgs(IEntityPlayer entity, PlayerMode from, PlayerMode to) : base(entity)
        {
            From = from;
            To = to;
        }
    }
}