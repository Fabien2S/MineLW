using System;
using System.Text.Json.Serialization;

namespace MineLW.API.Utils
{
    [Serializable]
    public struct GameVersion
    {
        [JsonPropertyName("name")]
        public string Name { get; }
        [JsonPropertyName("protocol")]
        public int Protocol { get; }

        public GameVersion(string name, int protocol)
        {
            Name = name;
            Protocol = protocol;
        }
    }
}