using DotNetty.Buffers;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.API.Worlds;
using MineLW.API.Worlds.Chunks;

namespace MineLW.API.Client
{
    public interface IClientConnection
    {
        void JoinGame(IClient client, IEntityPlayer player);
        void Disconnect(TextComponentString reason = null);
        
        void SendCustom(Identifier channel, IByteBuffer buffer);
        void SendMessage(TextComponent message);
        
        void Respawn(IWorldContext worldContext);
        void LoadChunk(ChunkPosition position, IChunk chunk);
        void UnloadChunk(ChunkPosition position);
    }
}