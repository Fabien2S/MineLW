using System.Collections.Generic;
using MineLW.API.Utils;

namespace MineLW.Networking
{
    public static class NetworkAdapter
    {
        public static GameVersion Default { get; private set; } = new GameVersion(string.Empty, 0);

        private static readonly Dictionary<int, NetworkState> Versions = new Dictionary<int, NetworkState>();

        public static bool IsSupported(GameVersion version)
        {
            return Versions.ContainsKey(version.Protocol);
        }

        public static void Register(GameVersion version, NetworkState state)
        {
            Versions[version.Protocol] = state;
            if (Default >= version)
                Default = version;
        }

        public static NetworkState Resolve(GameVersion version)
        {
            return Versions[version.Protocol];
        }
    }
}