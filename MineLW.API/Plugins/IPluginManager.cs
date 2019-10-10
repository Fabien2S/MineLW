namespace MineLW.API.Plugins
{
    public interface IPluginManager
    {
        void LoadPlugins();
        void UnloadPlugin(IPlugin plugin);

        void EnablePlugin(IPlugin plugin);
        void DisablePlugin(IPlugin plugin);
        void ReloadPlugin(IPlugin plugin);
    }
}