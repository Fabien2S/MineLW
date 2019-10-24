using System;

namespace MineLW.API.Commands.Exceptions
{
    public class CommandParseException : CommandException
    {
        public CommandParseException(string message) : base(message)
        {
        }

        public CommandParseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}