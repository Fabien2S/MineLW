﻿using System;
using System.Collections.Generic;

namespace MineLW.Debugging
{
    public class LogManager
    {
        private static readonly Dictionary<Type, Logger> Loggers = new Dictionary<Type, Logger>();
        
        public static LogLevel GlobalLevel = LogLevel.Info;
        
        public static Logger GetLogger<T>()
        {
            return GetLogger(typeof(T), GlobalLevel);
        }

        public static Logger GetLogger(Type type)
        {
            return GetLogger(type, GlobalLevel);
        }

        public static Logger GetLogger<T>(LogLevel level)
        {
            return GetLogger(typeof(T), level);
        }

        public static Logger GetLogger(Type type, LogLevel level)
        {
            if (Loggers.ContainsKey(type))
                return Loggers[type];
            return Loggers[type] = new Logger(type.Name, level);
        }
    }
}