using System;
using System.Reflection;
using System.Threading;
using MineLW.Adapters;
using MineLW.Adapters.MC498;
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
            // TODO implement that properly
            GameAdapter.Register<GameAdapter498>();
            
            Logger.Debug("The server is now ready to start.");
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

        private static void HandleCancelKeyPressed(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            _server?.Shutdown();
        }
    }
}