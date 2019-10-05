using System;
using System.Text.Json.Serialization;
using MineLW.API.Text;
using MineLW.API.Utils;

namespace MineLW.Networking.States.Status
{
    [Serializable]
    public struct ServerStatus
    {
        [JsonPropertyName("version")] public GameVersion GameVersion { get; set; }
        [JsonPropertyName("players")] public PlayerInfo Players { get; set; }
        [JsonPropertyName("description")] public TextComponent Description { get; set; }
    }

    [Serializable]
    public struct PlayerInfo
    {
        [JsonPropertyName("online")] public int Online { get; set; }
        [JsonPropertyName("max")] public int Max { get; set; }
        [JsonPropertyName("sample")] public PlayerProfile[] Players { get; set; }
    }
}