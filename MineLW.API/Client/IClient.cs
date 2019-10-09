using MineLW.API.Text;
using MineLW.API.Utils;

namespace MineLW.API.Client
{
    public interface IClient
    {
        PlayerProfile Profile { get; }
        
        void SendMessage(TextComponent message);
        void Kick(TextComponentString reason);
    }
}