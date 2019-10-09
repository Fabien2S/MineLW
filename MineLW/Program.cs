using System;
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
            // TODO implement that properly
            GameAdapter.Register<GameAdapter498>();

            Console.Title = GameServer.Name;
            Thread.CurrentThread.Name = "Main";

            Logger.Debug("Program started with {0} arguments", args.Length);

            Console.CancelKeyPress += HandleCancelKeyPressed;

            Logger.Debug("Configuring libraries...");
            ConfigureLibraries();

            Logger.Debug("The server is now ready to start.");
            _server = new GameServer();
            _server.Start();
        }

        private static void ConfigureLibraries()
        {
#if DEBUG
            LogManager.GlobalThreshold = LogLevel.Debug;
#else
            LogManager.GlobalThreshold = LogLevel.Info;
#endif


            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Error = (sender, args) =>
                {
                    var context = args.ErrorContext;
                    Logger.Error(context.Error, "An error occurred while processing JSON data: {0}");
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