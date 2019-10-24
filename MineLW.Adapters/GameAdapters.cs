using System.Collections.Generic;
using MineLW.API.Utils;
using NLog;

namespace MineLW.Adapters
{
    public static class GameAdapters
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly GameVersion Invalid = new GameVersion("Unknown", -1);
        private static readonly Dictionary<int, IGameAdapter> Versions = new Dictionary<int, IGameAdapter>();

        /// <summary>
        /// Gets the current version. This is the highest version registered into the <see cref="GameAdapters"/>
        /// </summary>
        public static GameVersion CurrentVersion { get; private set; } = Invalid;

        private static bool _locked;

        /// <summary>
        /// Register a game adapter
        /// </summary>
        /// <param name="gameAdapter">The game adapter</param>
        /// <remarks>Once <see cref="Lock"/>ed, the game adapter won't update the <see cref="CurrentVersion"/> property</remarks>
        public static void Register(IGameAdapter gameAdapter)
        {
            var version = gameAdapter.Version;
            Logger.Info("Using Minecraft version {0}", version);

            Versions[version.Protocol] = gameAdapter;

            if (!_locked && version >= CurrentVersion)
                CurrentVersion = version;
        }

        /// <summary>
        /// Lock the <see cref="GameAdapters"/>. This is used to ensure that the <see cref="CurrentVersion"/> won't change during runtime
        /// </summary>
        /// <returns>The <see cref="IGameAdapter"/> corresponding to <see cref="CurrentVersion"/>, or null</returns>
        public static IGameAdapter Lock()
        {
            if (_locked || CurrentVersion == Invalid)
                return null;

            _locked = true;
            return Resolve(CurrentVersion.Protocol);
        }

        /// <summary>
        /// Determines if the given protocol is supported
        /// </summary>
        /// <param name="protocol">The protocol id</param>
        /// <returns>true if the protocol is supported, false otherwise</returns>
        public static bool IsSupported(int protocol)
        {
            return Versions.ContainsKey(protocol);
        }

        /// <summary>
        /// Resolve a <see cref="IGameAdapter"/> for the given protocol
        /// </summary>
        /// <param name="protocol">The protocol id</param>
        /// <returns>The game adapter</returns>
        public static IGameAdapter Resolve(int protocol)
        {
            return Versions[protocol];
        }

        /// <summary>
        /// Gets the <see cref="IGameAdapter.Version"/> from the <see cref="IGameAdapter"/> with the given protocol id 
        /// </summary>
        /// <param name="protocol">The protocol id</param>
        /// <returns>The <see cref="GameVersion"/> of the given protocol id</returns>
        public static GameVersion GetVersion(int protocol)
        {
            return Resolve(protocol).Version;
        }
    }
}