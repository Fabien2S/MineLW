namespace MineLW.API.Worlds.Context
{
    public interface IWorldContext
    {
        IWorld World { get; }

        T GetOptionRaw<T>(WorldOption<T> option);
        T GetOption<T>(WorldOption<T> option);
        void SetOption<T>(WorldOption<T> option, T value);
    }
}