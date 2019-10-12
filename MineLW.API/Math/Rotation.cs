namespace MineLW.API.Math
{
    public struct Rotation
    {
        public static readonly Rotation Zero = new Rotation(0, 0);

        public readonly float Yaw;
        public readonly float Pitch;

        public Rotation(float yaw, float pitch)
        {
            Yaw = yaw % 360;
            Pitch = pitch % 360;
        }

        public static bool operator ==(Rotation a, Rotation b)
        {
            return MathHelper.AreRoughlyTheSame(a.Yaw, b.Yaw) && MathHelper.AreRoughlyTheSame(b.Yaw, b.Pitch);
        }

        public static bool operator !=(Rotation a, Rotation b)
        {
            return !(a == b);
        }
    }
}