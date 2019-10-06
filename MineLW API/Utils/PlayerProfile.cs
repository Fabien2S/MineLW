using System;

namespace MineLW.API.Utils
{
    [Serializable]
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

        [Serializable]
        public struct Property
        {
            public readonly string Name;
            public readonly string Value;
            public readonly string Signature;

            public Property(string name, string value, string signature)
            {
                Name = name;
                Value = value;
                Signature = signature;
            }
        }
    }
}