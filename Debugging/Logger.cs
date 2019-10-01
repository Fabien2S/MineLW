﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;

namespace MineLW.Debugging
{
    public class Logger
    {
        private static readonly ConsoleColor[] ColorByLevel =
        {
            ConsoleColor.DarkGray,
            ConsoleColor.White,
            ConsoleColor.Green,
            ConsoleColor.DarkYellow,
            ConsoleColor.Red
        };

        private readonly string _name;
        private LogLevel _level;

        internal Logger(string name, LogLevel level)
        {
            _name = name;
            _level = level;
        }

        private void Log(LogLevel level, string message, params object[] args)
        {
            var numericalLevel = (int) level;
            if (numericalLevel < (int) _level)
                return;

            var levelType = typeof(LogLevel);
            var levelName = Enum.GetName(levelType, level);
            var date = DateTime.Now;
            var currentThread = Thread.CurrentThread;

            var builder = new StringBuilder()
                .Append('[')
                .Append(date.ToString(CultureInfo.InvariantCulture))
                .Append(' ')
                .Append(levelName)
                .Append('/')
                .Append(_name)
                .Append('/')
                .Append(currentThread.Name)
                .Append(']')
                .Append(' ')
                .AppendFormat(CultureInfo.InvariantCulture, message, args);

            Console.ForegroundColor = ColorByLevel[numericalLevel];
            Console.WriteLine(builder.ToString());
            Console.ResetColor();
        }

        [Conditional("DEBUG")]
        public void Debug(string message, params object[] args)
        {
            Log(LogLevel.Debug, message, args);
        }

        public void Info(string message, params object[] args)
        {
            Log(LogLevel.Info, message, args);
        }

        public void Success(string message, params object[] args)
        {
            Log(LogLevel.Success, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            Log(LogLevel.Warn, message, args);
        }

        public void Error(string message, params object[] args)
        {
            Log(LogLevel.Error, message, args);
        }
    }
}