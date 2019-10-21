using System;

namespace MineLW.API.Commands
{
    public interface ICommandContext
    {
        CommandResult Result { get; set; }
        Exception Exception { get; set; }
    }
}