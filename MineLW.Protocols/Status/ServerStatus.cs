using System;
using MineLW.API.Players;
using MineLW.API.Text;
using MineLW.API.Utils;

namespace MineLW.Protocols.Status
{
    [Serializable]
    public struct ServerStatus
    {
        public readonly GameVersion Version;
        public readonly PlayerInfo Players;
        public readonly TextComponent Description;

        public ServerStatus(GameVersion version, PlayerInfo players, TextComponent description)
        {
            Version = version;
            Players = players;
            Description = description;
        }
    }

    [Serializable]
    public struct PlayerInfo
    {
        public readonly int Online;
        public readonly int Max;
        public readonly GameProfile[] Players;

        public PlayerInfo(int online, int max, GameProfile[] players)
        {
            Online = online;
            Max = max;
            Players = players;
        }
    }
}