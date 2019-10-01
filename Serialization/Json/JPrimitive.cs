﻿namespace MineLW.Serialization.Json
{
    public class JPrimitive<T> : JElement
    {
        private protected readonly T Value;

        protected JPrimitive(T value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator T(JPrimitive<T> obj)
        {
            return obj.Value;
        }
    }
}