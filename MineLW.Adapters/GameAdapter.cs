using System.Collections.Generic;
using MineLW.API.Utils;
using NLog;

namespace MineLW.Adapters
{
    public static class GameAdapter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static readonly GameVersion Invalid = new GameVersion("Unknown", -1);
        public static GameVersion Default { get; private set; } = Invalid;

        private static readonly Dictionary<int, IGameAdapter> Versions = new Dictionary<int, IGameAdapter>();

        public static void Register(IGameAdapter gameAdapter)
        {
            var version = gameAdapter.Version;
            Logger.Info("Registering game adapter {0}", version);

            Versions[version.Protocol] = gameAdapter;

            if (version >= Default)
                Default = version;
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