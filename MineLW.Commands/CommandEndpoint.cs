using System;
using MineLW.API.Commands.Exceptions;

namespace MineLW.Commands
{
    public struct CommandEndpoint
    {
        public readonly string Name;
        public readonly Argument[] Arguments;
        public readonly Delegate Delegate;

        public CommandEndpoint(string name, Argument[] arguments, Delegate @delegate)
        {
            Name = name;
            Arguments = arguments;
            Delegate = @delegate;
        }

        public struct Argument
        {
            public readonly string Name;
            public readonly Type Type;
            //public readonly ArgumentAttribute[] Attributes;

            public readonly bool HasDefaultValue;
            public readonly object DefaultValue;

            public Argument(string name, Type type, /*ArgumentAttribute[] attributes,*/ bool hasDefaultValue,
                object defaultValue)
            {
                Name = name;
                Type = type;
                //Attributes = attributes;

                HasDefaultValue = hasDefaultValue;
                DefaultValue = defaultValue;

                if (hasDefaultValue)
                    Validate(defaultValue);
            }

            public void Validate(object value)
            {
                /*foreach (var attribute in Attributes)
                {
                    if (attribute.IsValid(value))
                        continue;

                    throw new CommandParseException(
                        "Invalid value for argument \"" + Name + "\" (got: " + value + ", expected: " +
                        attribute.AllowedValues + ')'
                    );
                }*/
            }
        }
    }
}