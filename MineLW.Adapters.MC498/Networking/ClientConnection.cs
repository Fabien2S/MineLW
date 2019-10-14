using System;
using System.Numerics;
using DotNetty.Buffers;
using MineLW.Adapters.MC498.Networking.Client;
using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Math;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.API.Worlds;
using MineLW.API.Worlds.Chunks;
using MineLW.Networking;

namespace MineLW.Adapters.MC498.Networking
{
    public class ClientConnection : IClientConnection
    {
        private const string LevelType = "default";
        
        private readonly NetworkClient _networkClient;
        private IClient _client;

        public ClientConnection(NetworkClient networkClient)
        {
            _networkClient = networkClient;
        }

        public void Disconnect(TextComponentString reason = null)
        {
            _networkClient.Disconnect(reason);
        }

        public void JoinGame(IClient client, IEntityPlayer player)
        {
            if (_client != null)
                throw new NotSupportedException("Connection already initialized");

            _client = client;

            var worldContext = player.WorldContext;
            var environment = worldContext.GetOption(WorldOption.Environment);

            _networkClient.Send(new MessageClientInitGame.Message(
                player.Id,
                (byte) player.PlayerMode,
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
                _client.Player.PlayerMode,
                LevelType
            ));
        }

        public void SendCustom(Identifier channel, IByteBuffer buffer)
        {
            _networkClient.Send(new MessageClientCustomData.Message(channel, buffer));
        }

        public void SendMessage(TextComponent message)
        {
            _networkClient.Send(new MessageClientChatMessage.Message(message));
        }

        public void Teleport(Vector3 position, Rotation rotation, int id)
        {
            _networkClient.Send(new MessageClientPlayerTeleport.Message(
                position,
                rotation,
                id
            ));
        }

        public void UpdateView(ChunkPosition chunkPosition)
        {
            _networkClient.Send(new MessageClientUpdateViewPosition.Message(chunkPosition));
        }

        public void LoadChunk(ChunkPosition position, ChunkSnapshot chunkSnapshot)
        {
            var buffer = Unpooled.Buffer();

            var blockStorage = chunkSnapshot.BlockStorage;
            foreach (var storage in blockStorage)
            {
                if(storage == null)
                    continue;
                
                var blockCount = storage.BlockCount;
                if (blockCount == 0)
                    continue;

                // TODO add cross-version support
                buffer.WriteShort(blockCount);

                var blockPalette = storage.BlockPalette;
                buffer.WriteByte(blockPalette.BitsPerBlock);

                blockPalette.Serialize(buffer);
                storage.Serialize(buffer);
            }

            // biome
            for (var i = 0; i < 256; i++)
                buffer.WriteInt(0);

            _networkClient.Send(new MessageClientLoadChunk.Message(
                position,
                true,
                chunkSnapshot.SectionMask,
                chunkSnapshot.HeightMap,
                buffer
            ));
        }

        public void UnloadChunk(ChunkPosition position)
        {
            _networkClient.Send(new MessageClientUnloadChunk.Message(position));
        }
    }
}