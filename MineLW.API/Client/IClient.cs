using DotNetty.Buffers;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.API.Worlds;

namespace MineLW.API.Client
{
    public interface IClient : IUpdatable
    {       
        PlayerProfile Profile { get; }
        IEntityPlayer Player { get; }

        IClientWorld World { get; set; }

        void Init(IEntityPlayer player);
        void SendCustom(Identifier channel, IByteBuffer buffer);
        void SendMessage(TextComponent message);
        void Disconnect(TextComponentString reason);
        void Respawn(IWorldContext worldContext);
    }
}