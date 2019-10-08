namespace MineLW.API.Utils
{
    public static class Minecraft
    {
        private const string Namespace = "minecraft";

        public static class Units
        {
            public static float ToMeterPerSeconds(float minecraftMetric)
            {
                return (float) (43.178 * minecraftMetric - 0.02141);
            }
        }

        public static class Channels
        {
            public static readonly Identifier Brand = new Identifier(Namespace, "brand");
        }

        public static Identifier CreateKey(string key)
        {
            return new Identifier(Namespace, key);
        }
    }
}