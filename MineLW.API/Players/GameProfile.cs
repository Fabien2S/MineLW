using System;

namespace MineLW.API.Players
{
    [Serializable]
    public struct GameProfile
    {
        public readonly Guid Id;
        public readonly string Name;
        public readonly Property[] Properties;
        
        public GameProfile(Guid id, string name, Property[] properties)
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