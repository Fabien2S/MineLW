namespace MineLW.API.Utils
{
    public static class Minecraft
    {
        private const string Namespace = "minecraft";

        public static class Units
        {
            /// <summary>
            /// Convert a Minecraft metric value (e.g. a player's walk speed) and convert it to meter per seconds
            /// </summary>
            /// <param name="mcMetricValue"></param>
            /// <returns>The Minecraft metric value in m/s</returns>
            public static float ToMeterPerSeconds(float mcMetricValue)
            {
                return (float) (43.178 * mcMetricValue - 0.02141);
            }
        }

        public static class Channels
        {
            public static readonly Identifier Brand = new Identifier(Namespace, "brand");
        }

        public static Identifier CreateIdentifier(string key)
        {
            return new Identifier(Namespace, key);
        }
    }
}