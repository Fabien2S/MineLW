namespace MineLW.API
{
    public static partial class Minecraft
    {
        public static class Units
        {
            public static class World
            {
                public const int Height = 256;
            }

            public static class Chunk
            {
                public const int Size = 16;

                public const int SectionHeight = 16;
                public const int SectionCount = World.Height / SectionHeight;
                public const int SectionBlockCount = Size * SectionCount * Size;
            }

            /// <summary>
            /// Convert an entity walk speed to meter per seconds
            /// </summary>
            /// <param name="f">the value to convert</param>
            /// <returns>the value in m/s</returns>
            public static float ToMeterPerSeconds(float f)
            {
                return (float) (43.178 * f - 0.02141);
            }
        }
    }
}