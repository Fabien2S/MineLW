using System;

namespace MineLW.API.Entities.Living.Player
{
    [Serializable]
    public struct PlayerProfile
    {
        public readonly Guid Id;
        public readonly string Name;

        public PlayerProfile(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
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