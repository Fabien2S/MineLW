namespace MineLW.API.Blocks.Properties
{
    public interface IBlockProperty
    {
        string Name { get; }
        int ValueCount { get; }

        object Parse(string source);
        int GetIndex(object value);
        object GetValue(int index);
    }
}