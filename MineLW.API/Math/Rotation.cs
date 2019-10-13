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

        public bool Equals(Rotation other)
        {
            return MathHelper.AreRoughlyTheSame(Yaw, other.Yaw) && MathHelper.AreRoughlyTheSame(Yaw, other.Pitch);
        }

        public override bool Equals(object obj)
        {
            return obj is Rotation other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Yaw, Pitch).GetHashCode();
        }
        
        public static bool operator ==(Rotation a, Rotation b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Rotation a, Rotation b)
        {
            return !a.Equals(b);
        }
    }
}