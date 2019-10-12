using System;
using System.Collections.Generic;
using MineLW.API.Utils;
using NLog;

namespace MineLW.Adapters
{
    public static class GameAdapter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static readonly GameVersion Invalid = new GameVersion("Unknown", -1);

        public static GameVersion ServerVersion { get; private set; } = Invalid;
        public static IGameAdapter ServerAdapter { get; private set; }

        private static readonly Dictionary<int, IGameAdapter> Versions = new Dictionary<int, IGameAdapter>();

        public static void Register(IGameAdapter gameAdapter)
        {
            if (ServerAdapter != null)
                throw new InvalidOperationException("GameAdapter is locked");

            var version = gameAdapter.Version;
            Logger.Info("Using Minecraft version {0}", version);

            Versions[version.Protocol] = gameAdapter;

            if (version >= ServerVersion)
                ServerVersion = version;
        }

        public static void Lock()
        {
            ServerAdapter = Resolve(ServerVersion.Protocol);
        }

        public static bool IsSupported(int protocol)
        {
            return Versions.ContainsKey(protocol);
        }

        public static IGameAdapter Resolve(int protocol)
        {
            return Versions[protocol];
        }

        public static GameVersion GetVersion(int protocol)
        {
            return Resolve(protocol).Version;
        }
    }
}