using System;

namespace MineLW.API.Physics
{
    [Flags]
    public enum MotionTypes : byte
    {
        None = 0b00,
        Position = 0b01,
        Rotation = 0b10
    }
}