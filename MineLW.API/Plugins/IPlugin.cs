namespace MineLW.API.Plugins
{
    public interface IPlugin
    {
        string Name { get; }
        string Author { get; }

        void Initialize();
        void Shutdown();
        void Reload();
    }
}