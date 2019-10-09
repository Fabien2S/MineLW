using System.Collections.Generic;
using MineLW.API.Utils;
using NLog;

namespace MineLW.Adapter
{
    public static class GameAdapter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static GameVersion Default { get; private set; } = new GameVersion(string.Empty, 0);

        private static readonly Dictionary<int, IGameAdapter> Versions = new Dictionary<int, IGameAdapter>();

        public static bool IsSupported(GameVersion version)
        {
            return Versions.ContainsKey(version.Protocol);
        }

        public static void Register<T>() where T : IGameAdapter, new()
        {
            var adapter = new T();

            var version = adapter.Version;
            Logger.Debug("Registering version {0}", version.Protocol);

            Versions[version.Protocol] = adapter;

            if (Default >= version)
                Default = version;
        }

        public static IGameAdapter Resolve(GameVersion version)
        {
            return Versions[version.Protocol];
        }
    }
}