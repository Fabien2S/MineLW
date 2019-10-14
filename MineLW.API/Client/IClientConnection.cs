using System.Numerics;
using DotNetty.Buffers;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Math;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.API.Worlds;
using MineLW.API.Worlds.Chunks;

namespace MineLW.API.Client
{
    public interface IClientConnection
    {
        void Disconnect(TextComponentString reason = null);
        
        void JoinGame(IClient client, IEntityPlayer player);
        void Respawn(IWorldContext worldContext);
        
        void SendCustom(Identifier channel, IByteBuffer buffer);
        void SendMessage(TextComponent message);

        void Teleport(Vector3 position, Rotation rotation, int id);
        void UpdateView(ChunkPosition chunkPosition);
        
        void LoadChunk(ChunkPosition position, ChunkSnapshot chunkSnapshot);
        void UnloadChunk(ChunkPosition position);
    }
}