using MineLW.API.Blocks;
using MineLW.API.Entities;
using MineLW.API.Math;

namespace MineLW.API.Worlds.Context
{
    public interface IWorldContext
    {
        IWorld World { get; }
        IEntityManager EntityManager { get; }

        IBlockState GetBlock(Vector3Int position);
        void SetBlock(Vector3Int position, IBlockState blockState);

        T GetOptionRaw<T>(WorldOption<T> option);
        T GetOption<T>(WorldOption<T> option);
        void SetOption<T>(WorldOption<T> option, T value);
    }
}