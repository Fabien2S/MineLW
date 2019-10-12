using MineLW.API.Utils;

namespace MineLW.API
{
    public static partial class Minecraft
    {
        private const string Namespace = "minecraft";

        public static Identifier CreateIdentifier(string key)
        {
            return new Identifier(Namespace, key);
        }
    }
}