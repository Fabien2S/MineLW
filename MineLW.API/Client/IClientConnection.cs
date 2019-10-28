using System.Collections.Generic;
using System.Numerics;
using MineLW.API.Entities;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Math;
using MineLW.API.Physics;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.API.Worlds;
using MineLW.API.Worlds.Chunks;

namespace MineLW.API.Client
{
    public interface IClientConnection
    {
        #region Client
        void Disconnect(TextComponent reason = null);
        void UpdateView(ChunkPosition chunkPosition);
        void SendCustom(Identifier channel, byte[] data);
        void SendPingRequest(long pingId);
        #endregion

        #region Client player
        void SendMessage(TextComponent message);
        void Spawn(IClient client, IEntityPlayer player);
        void Respawn(IWorldContext worldContext);
        void Teleport(Vector3 position, Rotation rotation, int id);
        void RotateToward(EntityReference reference, Vector3 position);
        void RotateToward(EntityReference reference, IEntity entity, EntityReference targetReference);
        #endregion
        
        #region Entities
        void SpawnEntity(IEntity entity);
        void DestroyEntities(IEnumerable<IEntity> entities);
        void MoveEntity(IEntity entity, Vector3 delta, MotionTypes motionTypes);
        #endregion
        


        #region World
        void LoadChunk(ChunkPosition position, IChunk chunk);
        void UnloadChunk(ChunkPosition position);
        #endregion
    }
}