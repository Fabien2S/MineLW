using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using MineLW.Adapters;
using MineLW.API.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;

namespace MineLW.Server
{
    internal static class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static IServer _server;

        private static void Main(string[] args)
        {
            var currentThread = Thread.CurrentThread;
            currentThread.Name = "Server";
            
            Logger.Debug("Fetching {0} argument(s)", args.Length);

            var executingAssembly = Assembly.GetExecutingAssembly();
            var assemblyName = executingAssembly.GetName();
            Console.Title = assemblyName.Name + " " + assemblyName.Version;
            Console.CancelKeyPress += HandleCancelKeyPressed;

            Logger.Info("Configuring libraries...");
            ConfigureLibraries();

            Logger.Info("Loading game adapters...");
            LoadGameAdapters();
            var serverAdapter = GameAdapters.Lock();
            if (serverAdapter == null)
            {
                Logger.Error("No game adapter found");
                return;
            }

            _server = new GameServer(serverAdapter);
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

            if(!Directory.Exists(adaptersPath))
                Directory.CreateDirectory(adaptersPath);

            var files = Directory.EnumerateFiles(adaptersPath, "*.dll", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var defaultAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                var exportedTypes = defaultAssembly.GetExportedTypes();
                foreach (var exportedType in exportedTypes)
                {
                    if (!typeof(IGameAdapter).IsAssignableFrom(exportedType))
                        continue;
                    if (exportedType.IsAbstract)
                        continue;

                    var gameAdapter = Activator.CreateInstance(exportedType);
                    GameAdapters.Register((IGameAdapter) gameAdapter);
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