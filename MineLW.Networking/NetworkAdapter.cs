using System.Collections.Generic;
using MineLW.API.Utils;
using MineLW.Networking.States.Game;

namespace MineLW.Networking
{
    public static class NetworkAdapter
    {
        public static GameVersion Default { get; private set; } = new GameVersion(string.Empty, 0);

        private static readonly Dictionary<int, GameState> Versions = new Dictionary<int, GameState>();

        public static bool IsSupported(GameVersion version)
        {
            return Versions.ContainsKey(version.Protocol);
        }

        public static void Register<T>(GameVersion version) where T : GameState, new()
        {
            Versions[version.Protocol] = new T();
            if (Default >= version)
                Default = version;
        }

        public static GameState Resolve(GameVersion version)
        {
            return Versions[version.Protocol];
        }
    }
}