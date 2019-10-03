using System;
using System.Threading;
using MineLW.API;

namespace MineLW
{
    internal static class Program
    {
        private static IServer _server;

        private static void Main(string[] args)
        {
            Console.Title = GameServer.Name;
            Thread.CurrentThread.Name = "Main";

            Console.CancelKeyPress += HandleCancelKeyPressed;

            _server = new GameServer();
            _server.Start();
        }

        private static void HandleCancelKeyPressed(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            _server?.Shutdown();
        }
    }
}