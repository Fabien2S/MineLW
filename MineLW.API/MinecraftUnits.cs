namespace MineLW.API
{
    public static partial class Minecraft
    {
        public static class Units
        {
            public static class Chunk
            {
                public const int Size = 16;
                public const int Height = 256;

                public const int SectionHeight = 16;
                public const int SectionCount = Height / SectionHeight;
                public const int SectionBlockCount = Size * SectionCount * Size;
            }

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
    }
}