using System;
using System.IO;
using System.Reflection;
using System.Threading;
using McMaster.NETCore.Plugins;
using MineLW.Adapters;
using MineLW.API;
using MineLW.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;

namespace MineLW
{
    internal static class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static IServer _server;

        private static void Main(string[] args)
        {
            Logger.Debug("Program started with {0} arguments", args.Length);

            var currentThread = Thread.CurrentThread;
            currentThread.Name = "Main";

            var executingAssembly = Assembly.GetExecutingAssembly();
            var assemblyName = executingAssembly.GetName();
            Console.Title = assemblyName.Name + " " + assemblyName.Version;
            Console.CancelKeyPress += HandleCancelKeyPressed;

            Logger.Info("Configuring libraries...");
            ConfigureLibraries();

            Logger.Info("Loading game adapters...");
            LoadGameAdapters();
            if (GameAdapter.Default == GameAdapter.Invalid)
            {
                Logger.Error("No game adapter found");
                return;
            }

            _server = new GameServer();
            Console.Title = _server.Name;

            _server.Start();
        }

        private static void ConfigureLibraries()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Error = (sender, args) =>
                {
                    var context = args.ErrorContext;
                    Logger.Error(context.Error, "Unable to process JSON data");
                    context.Handled = true;
                }
            };
        }

        private static void LoadGameAdapters()
        {
            var adaptersPath = Path.Combine(Environment.CurrentDirectory, "adapters");

            Directory.CreateDirectory(adaptersPath);
            
            var files = Directory.EnumerateFiles(adaptersPath, "*.dll", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var pluginLoader = PluginLoader.CreateFromAssemblyFile(file, new []{typeof(IGameAdapter)});
                var defaultAssembly = pluginLoader.LoadDefaultAssembly();
                var exportedTypes = defaultAssembly.GetExportedTypes();
                foreach (var exportedType in exportedTypes)
                {
                    if (!typeof(IGameAdapter).IsAssignableFrom(exportedType))
                        continue;
                    if (exportedType.IsAbstract)
                        continue;
                    
                    var gameAdapter = Activator.CreateInstance(exportedType);
                    GameAdapter.Register((IGameAdapter) gameAdapter);
                }
            }
        }

        private static void HandleCancelKeyPressed(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            _server?.Shutdown();
        }
    }
}