using MineLW.API.Text;

namespace MineLW.API.Client
{
    public interface IClient
    {
        void SendMessage(TextComponent message);
        void Kick(TextComponentString reason);
    }
}