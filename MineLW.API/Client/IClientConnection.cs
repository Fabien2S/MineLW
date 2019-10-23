using System.Numerics;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.API.Worlds;
using MineLW.API.Worlds.Chunks;

namespace MineLW.API.Client
{
    public interface IClientConnection
    {
        void SendPingRequest(long pingId);
        void Disconnect(TextComponent reason = null);
        
        void JoinGame(IClient client, IEntityPlayer player);
        void Respawn(IWorldContext worldContext);
        
        void SendCustom(Identifier channel, byte[] data);
        void SendMessage(TextComponent message);

        void Teleport(Vector3 position, Quaternion rotation, int id);
        void UpdateView(ChunkPosition chunkPosition);
        
        void LoadChunk(ChunkPosition position, IChunk chunk);
        void UnloadChunk(ChunkPosition position);
    }
}