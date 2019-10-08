namespace MineLW.API.Blocks
{
    public interface IBlockState
    {
        int Id { get; }
        IBlock Type { get; }
        object[] Properties { get; }
    }
}