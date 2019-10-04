namespace MineLW.API.Utils
{
    public struct Version
    {
        public readonly string Name;
        public readonly int Protocol;

        public Version(string name, int protocol)
        {
            Name = name;
            Protocol = protocol;
        }
    }
}