using System.Collections.Generic;
using MineLW.API.Commands;

namespace MineLW.Commands
{
    public class CommandNode : ICommandNode
    {
        public string Name { get; }

        private readonly Dictionary<string, ICommandNode> _nodes = new Dictionary<string, ICommandNode>();
        private readonly Dictionary<string, CommandEndpoint> _endpoints = new Dictionary<string, CommandEndpoint>();

        public CommandNode(string name)
        {
            Name = name;

            //CreateEndpoints();
        }

        public void Register<T>() where T : ICommandNode, new()
        {
            var command = new T();
            _nodes[command.Name] = command;
        }

        /*private void CreateEndpoints()
        {
            _endpoints.Clear();

            var commandType = GetType();
            var methods = commandType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach (var methodInfo in methods)
            {
                var endpoint = methodInfo.GetCustomAttribute<CmdEndpoint>();
                if (endpoint == null)
                    continue;

                var parameters = methodInfo.GetParameters();
                if (parameters.Length < 1)
                    continue;
                if(!typeof(ICommandEmitter).IsAssignableFrom(parameters[0].ParameterType))
                    throw new CommandParseException("");

                var firstParameter = parameters.First();
                if (firstParameter.ParameterType != typeof(ICommandEmitter))
                    continue;

                var delegateType = Expression.GetDelegateType(parameters
                    .Select(p => p.ParameterType)
                    .Append(methodInfo.ReturnType)
                    .ToArray()
                );
                var invokeDelegate = Delegate.CreateDelegate(delegateType, this, methodInfo);

                Array.Copy(parameters, 1, parameters, 0, parameters.Length - 1);
                var arguments = parameters.Select(p => new CommandEndpoint.Argument(
                    p.Name,
                    p.ParameterType,
                    (ArgumentAttribute[]) Attribute.GetCustomAttributes(p, typeof(ArgumentAttribute)),
                    p.HasDefaultValue,
                    p.DefaultValue
                )).ToArray();

                var commandEndpoint = new CommandEndpoint(
                    endpoint.Name,
                    arguments,
                    invokeDelegate
                );

                _endpoints[endpoint.Name] = commandEndpoint;
            }
        }

        internal void Execute(ICommandManager commandManager, ICommandContext context, StringReader reader)
        {
            var literal = reader.ReadUnquotedString();
            reader.ConsumeWhitespaces();
            
            if (_nodes.ContainsKey(literal))
            {
                var node = _nodes[literal];
                node.Execute(commandManager, context, reader);
                return;
            }
            
            var hasEndpoint = _endpoints.ContainsKey(literal);
            if (hasEndpoint || _defaultEndpoint != null)
            {
                var endpoint = hasEndpoint ? _endpoints[literal] : _defaultEndpoint.Value;
                var arguments = endpoint.Arguments;

                var methodArguments = new object[arguments.Length];
                methodArguments[0] = emitter;
                
                for (var i = 1; i < arguments.Length; i++)
                {
                    var argument = arguments[i - 1];
                    reader.SkipWhitespace();

                    var serializer = commandManager.GetSerializer(argument.Type);
                    Assert.NonNull(serializer, "Serializer is null ( " + argument.Type.Name + ')');

                    object value;
                    try
                    {
                        value = serializer.Deserialize(argument.Type, reader);
                    }
                    catch (FormatException e)
                    {
                        if (argument.HasDefaultValue)
                            value = argument.DefaultValue;
                        else
                            throw new CommandParseException("Missing \"" + argument.Name + "\" argument", e);
                    }

                    argument.Validate(value);

                    methodArguments[i] = value;
                }
                
                endpoint.Delegate.DynamicInvoke(methodArguments);
                return;
            }
            
            throw new CommandException("Unknown command \"" + reader.InputRead + '"');
        }*/
    }
}