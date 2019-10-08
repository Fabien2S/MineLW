namespace MineLW.API.Worlds
{
    public abstract class WorldOption
    {
        public static readonly WorldOption Time = new WorldOption<int>(0);
        public static readonly WorldOption DaylightCycle = new WorldOption<bool>(false);
        public static readonly WorldOption Raining = new WorldOption<bool>(false);
    }

    public class WorldOption<T> : WorldOption
    {
        public readonly T DefaultValue;

        public WorldOption(T defaultValue)
        {
            DefaultValue = defaultValue;
        }
    }
}