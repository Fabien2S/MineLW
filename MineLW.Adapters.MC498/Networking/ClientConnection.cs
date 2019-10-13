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

        public void JoinGame(IClient client, IEntityPlayer player)
        {
            if (_client !=null)
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
            _networkClient.Send(new MessageClientPlayerTeleport.Message(
                Vector3.Zero,
                Rotation.Zero,
                0
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

        public void Disconnect(TextComponentString reason = null)
        {
            _networkClient.Disconnect(reason);
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

        public void LoadChunk(ChunkPosition position, IChunk chunk)
        {
            throw new NotImplementedException();
        }

        public void UnloadChunk(ChunkPosition position)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            _networkClient.Disconnect();
        }
    }
}