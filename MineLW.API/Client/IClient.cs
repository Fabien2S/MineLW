using DotNetty.Buffers;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.API.Utils;

namespace MineLW.API.Client
{
    public interface IClient : IUpdatable
    {
        PlayerProfile Profile { get; }

        void Init(IEntityPlayer player);
        void SendCustom(Identifier channel, IByteBuffer buffer);
        void SendMessage(TextComponent message);
        void Disconnect(TextComponentString reason);
    }
}