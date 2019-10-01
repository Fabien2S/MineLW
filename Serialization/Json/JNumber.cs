﻿namespace MineLW.Serialization.Json
{
    public class JNumber : JPrimitive<float>
    {
        public JNumber(float value) : base(value)
        {
        }
        
        public static implicit operator float(JNumber obj)
        {
            return obj.Value;
        }
        
        public static implicit operator int(JNumber obj)
        {
            return (int) obj.Value;
        }
    }
}