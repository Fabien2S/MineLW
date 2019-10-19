using MineLW.API;
using MineLW.API.Plugins;

namespace MineLW.Plugins
{
    public class PluginManager : IPluginManager
    {
        private readonly IServer _server;
        private readonly string _pluginsFolder;

        //private readonly Dictionary<IPlugin, PluginLoader> _plugins = new Dictionary<IPlugin, PluginLoader>();

        public PluginManager(IServer server, string pluginsFolder)
        {
            _server = server;
            _pluginsFolder = pluginsFolder;
        }

        public void LoadPlugins()
        {
            /*var files = Directory.EnumerateFiles(_pluginsFolder, "*.dll");
            foreach (var file in files)
            {
                var pluginLoader = PluginLoader.CreateFromAssemblyFile(file, new[] {typeof(IPlugin)});
                var defaultAssembly = pluginLoader.LoadDefaultAssembly();
                var exportedTypes = defaultAssembly.GetExportedTypes();
                foreach (var type in exportedTypes)
                {
                    if (!typeof(IPlugin).IsAssignableFrom(type))
                        continue;
                    if (type.IsAbstract)
                        continue;

                    var plugin = (IPlugin) Activator.CreateInstance(type);
                    _plugins[plugin] = pluginLoader;
                }
            }*/
        }

        public void UnloadPlugin(IPlugin plugin)
        {
            /*if (!_plugins.ContainsKey(plugin))
                throw new ArgumentNullException(nameof(plugin), "Plugin not registered");

            var loader = _plugins[plugin];
            loader.Dispose();*/
        }

        public void EnablePlugin(IPlugin plugin)
        {
        }

        public void DisablePlugin(IPlugin plugin)
        {
        }

        public void ReloadPlugin(IPlugin plugin)
        {
        }
    }
}