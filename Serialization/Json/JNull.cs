﻿namespace MineLW.Serialization.Json
{
    public class JNull : JElement
    {
        public static readonly JNull Null = new JNull();
        
        private JNull() {}
    }
}