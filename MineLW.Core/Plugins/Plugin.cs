using MineLW.API.Plugins;

namespace MineLW.Plugins
{
    public abstract class Plugin : IPlugin
    {
        public abstract string Name { get; }
        public abstract string Author { get; }

        public abstract void Initialize();
        public abstract void Shutdown();
        public abstract void Reload();
    }
}