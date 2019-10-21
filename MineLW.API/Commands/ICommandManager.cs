namespace MineLW.API.Commands
{
    public interface ICommandManager
    {
        void Register<T>() where T : ICommand, new();
        void Register(ICommand command);
    }
}