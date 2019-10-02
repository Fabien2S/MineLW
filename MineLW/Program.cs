using System;
using System.Threading;

namespace MineLW
{
    internal static class Program
    {
        private static GameServer _server;

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