using MineLW.API.Entities;

namespace MineLW.API.Worlds
{
    public interface IWorldContext
    {
        IWorld World { get; }
        IEntityManager EntityManager { get; }

        T GetOptionRaw<T>(WorldOption<T> option);
        T GetOption<T>(WorldOption<T> option);
        void SetOption<T>(WorldOption<T> option, T value);
    }
}