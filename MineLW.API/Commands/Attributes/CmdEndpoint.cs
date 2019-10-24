using System;

namespace MineLW.API.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CmdEndpoint : Attribute
    {
        public readonly string Name;

        public CmdEndpoint(string name)
        {
            Name = name;
        }
    }
}