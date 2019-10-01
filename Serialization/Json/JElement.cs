﻿using System;

namespace MineLW.Serialization.Json
{
    public class JElement
    {
        public T Get<T>() where T : JElement
        {
            if (!(this is T t))
                throw new FormatException(typeof(T).Name + " expected");
            return t;
        }
    }
}