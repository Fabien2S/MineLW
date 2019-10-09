namespace MineLW.API.Plugin
{
    public interface IPluginManager
    {
        void LoadPlugin(IPlugin plugin);
        void UnloadPlugin(IPlugin plugin);

        void EnablePlugin(IPlugin plugin);
        void DisablePlugin(IPlugin plugin);
        void ReloadPlugin(IPlugin plugin);
    }
}