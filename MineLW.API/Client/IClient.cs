using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.API.Utils;

namespace MineLW.API.Client
{
    public interface IClient
    {
        PlayerProfile Profile { get; }

        void Init(IEntityPlayer player);
        void SendMessage(TextComponent message);
        void Disconnect(TextComponentString reason);
    }
}