using System;

namespace MineLW.API.Entities.Living.Player
{
    [Serializable]
    public struct PlayerProfile
    {
        public readonly Guid Id;
        public readonly string Name;
        public readonly Property[] Properties;
        
        public PlayerProfile(Guid id, string name, Property[] properties)
        {
            Id = id;
            Name = name;
            Properties = properties;
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