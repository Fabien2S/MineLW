﻿using System;

namespace MineLW.Serialization.Json
{
    public class JString : JPrimitive<string>
    {
        public JString(string value) : base(value)
        {
        }

        public T Enum<T>()
        {
            var t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException("Not an enum type");
            return (T) System.Enum.Parse(t, this, true);
        }

        public Guid Guid()
        {
            return System.Guid.Parse(Value);
        }

        public static implicit operator JString(Guid obj)
        {
            return new JString(obj.ToString());
        }
    }
}