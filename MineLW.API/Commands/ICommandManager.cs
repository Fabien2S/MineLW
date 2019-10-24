using MineLW.API.IO;

namespace MineLW.API.Commands
{
    public interface ICommandManager
    {
        void Register<T>() where T : ICommandNode, new();
        void Execute(ICommandEmitter emitter, StringReader reader);
    }
}