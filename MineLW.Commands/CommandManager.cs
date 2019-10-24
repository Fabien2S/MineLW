using MineLW.API.Commands;
using MineLW.API.IO;

namespace MineLW.Commands
{
    public class CommandManager : ICommandManager
    {
        private readonly CommandNode _root = new CommandNode(string.Empty);

        public void Register<T>() where T : ICommandNode, new()
        {
            _root.Register<T>();
        }

        public void Execute(ICommandEmitter emitter, StringReader reader)
        {
        }
    }
}