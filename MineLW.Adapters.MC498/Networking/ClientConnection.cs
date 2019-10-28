using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DotNetty.Buffers;
using MineLW.Adapters.MC498.Networking.Client;
using MineLW.API;
using MineLW.API.Client;
using MineLW.API.Entities;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Math;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.API.Worlds;
using MineLW.API.Worlds.Chunks;
using MineLW.Networking;
using NLog;

namespace MineLW.Adapters.MC498.Networking
{
    public class ClientConnection : IClientConnection
    {
        private const string LevelType = "default";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        private readonly NetworkClient _networkClient;
        private IClient _client;

        public ClientConnection(NetworkClient networkClient)
        {
            _networkClient = networkClient;
        }

        public void Disconnect(TextComponent reason = null)
        {
            _networkClient.Disconnect(reason);
        }

        public void UpdateView(ChunkPosition chunkPosition)
        {
            _networkClient.Send(new MessageClientUpdateViewPosition.Message(chunkPosition));
        }

        public void SendCustom(Identifier channel, byte[] data)
        {
            _networkClient.Send(new MessageClientCustomData.Message(channel, data));
        }

        public void SendPingRequest(long pingId)
        {
            _networkClient.Send(new MessageClientPingRequest.Message(pingId));
        }

        public void SendMessage(TextComponent message)
        {
            _networkClient.Send(new MessageClientChatMessage.Message(message));
        }

        public void Spawn(IClient client, IEntityPlayer player)
        {
            if (_client != null)
                throw new NotSupportedException("Connection already initialized");

            _client = client;

            _networkClient.Send(new MessageClientPlayerInfo.Message(
                MessageClientPlayerInfo.Action.AddPlayer,
                new []{player}
            ));

            var worldContext = player.WorldContext;
            var environment = worldContext.GetOption(WorldOption.Environment);
            _networkClient.Send(new MessageClientInitGame.Message(
                player.Id,
                (byte) player.GameMode,
                environment.Id,
                0,
                LevelType,
                client.World.RenderDistance,
                false
            ));
        }

        public void Respawn(IWorldContext worldContext)
        {
            var environment = worldContext.GetOption(WorldOption.Environment);
            _networkClient.Send(new MessageClientRespawn.Message(
                environment.Id,
                _client.Player.GameMode,
                LevelType
            ));
        }

        public void Teleport(Vector3 position, Rotation rotation, int id)
        {
            _networkClient.Send(new MessageClientPlayerTeleport.Message(
                position,
                rotation,
                id
            ));
        }

        public void RotateToward(EntityReference reference, Vector3 position)
        {
            _networkClient.Send(new MessageClientRotateToward.Message(
                reference,
                position,
                false,
                0,
                EntityReference.Feet
            ));
        }

        public void RotateToward(EntityReference reference, IEntity entity, EntityReference targetReference)
        {
            _networkClient.Send(new MessageClientRotateToward.Message(
                reference,
                entity.Position,
                true,
                entity.Id,
                targetReference
            ));
        }

        public void SpawnEntity(IEntity entity)
        {
            switch (entity)
            {
                case IEntityPlayer player:
                    _networkClient.Send(new MessageClientPlayerInfo.Message(
                        MessageClientPlayerInfo.Action.AddPlayer,
                        new []{player}
                    ));
                    _networkClient.Send(new MessageClientSpawnPlayer.Message(
                        entity.Id,
                        entity.Uuid,
                        entity.Position,
                        entity.Rotation
                    ));
                    
                    if(player.IsListed)
                        break;
                    _networkClient.Send(new MessageClientPlayerInfo.Message(
                        MessageClientPlayerInfo.Action.RemovePlayer,
                        new []{player}
                    ));
                    break;
                default:
                    Logger.Warn("Undefined entity type \"{0}\"", entity);
                    return;
            }
        }

        public void DestroyEntities(IEnumerable<IEntity> entities)
        {
            _networkClient.Send(new MessageClientDestroyEntities.Message(
                entities
                    .Select(e => e.Id)
                    .ToArray()
            ));
        }

        public void LoadChunk(ChunkPosition position, IChunk chunk)
        {
            var buffer = Unpooled.Buffer();
            
            var sectionMask = 0;
            for (var y = 0; y < Minecraft.Units.Chunk.SectionCount; y++)
            {
                var section = chunk[y];
                if (section == null)
                    continue;

                var blockStorage = section.BlockStorage;
                var blockCount = blockStorage.BlockCount;
                if (blockCount == 0)
                    continue;

                sectionMask |= 1 << y;
                    
                buffer.WriteShort(blockCount);

                var blockPalette = blockStorage.BlockPalette;
                buffer.WriteByte(blockPalette.BitsPerBlock);

                blockPalette.Serialize(buffer);
                blockStorage.Serialize(buffer);
            }

            // TODO send biome to the client
            for (var i = 0; i < 256; i++)
                buffer.WriteInt(0);

            _networkClient.Send(new MessageClientLoadChunk.Message(
                position,
                true,
                sectionMask,
                chunk.HeightMap,
                buffer
            ));
        }
        public void UnloadChunk(ChunkPosition position)
        {
            _networkClient.Send(new MessageClientUnloadChunk.Message(position));
        }
    }
}