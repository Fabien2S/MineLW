using MineLW.API.Text;

namespace MineLW.API.Commands
{
    public interface ICommandEmitter
    {
        void SendMessage(TextComponent component);
    }
}