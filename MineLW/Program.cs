using System;
using System.Threading;
using MineLW.API;
using MineLW.Debugging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MineLW
{
    internal static class Program
    {
        private static readonly Logger Logger = LogManager.GetLogger(typeof(Program));

        private static IServer _server;

        private static void Main(string[] args)
        {
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
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Error = (sender, args) =>
                {
                    var context = args.ErrorContext;
                    Logger.Error("An error occurred while processing JSON data: {0}", context.Error);
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