using System;

namespace MineLW.API.Commands.Exceptions
{
    public class CommandRegisterException : CommandException
    {
        public CommandRegisterException(string message) : base(message)
        {
        }

        public CommandRegisterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}