using System;

namespace MineLW.API.Utils
{
    public struct PlayerProfile
    {
        public readonly string Name;
        public readonly Guid Uuid;

        public PlayerProfile(string name, Guid uuid)
        {
            Name = name;
            Uuid = uuid;
        }

        public override string ToString()
        {
            return Name + '{' + Uuid + ')';
        }
    }
}