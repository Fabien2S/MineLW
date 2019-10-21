using System;
using System.Collections.Generic;
using MineLW.API.Blocks;
using MineLW.API.Utils;
using NLog;

namespace MineLW.Adapters
{
    public static class GameAdapters
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly GameVersion Invalid = new GameVersion("Unknown", -1);
        private static readonly Dictionary<int, IGameAdapter> Versions = new Dictionary<int, IGameAdapter>();

        public static Func<IBlockManager> BlockManagerSupplier { get; set; }

        public static GameVersion ServerVersion { get; private set; } = Invalid;

        private static bool _locked;

        public static void Register(IGameAdapter gameAdapter)
        {
            if (_locked)
                throw new InvalidOperationException("GameAdapter is locked");

            var version = gameAdapter.Version;
            Logger.Info("Using Minecraft version {0}", version);

            Versions[version.Protocol] = gameAdapter;

            if (version >= ServerVersion)
                ServerVersion = version;
        }

        public static IGameAdapter Lock()
        {
            if (_locked || ServerVersion == Invalid)
                return null;

            _locked = true;
            return Resolve(ServerVersion.Protocol);
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