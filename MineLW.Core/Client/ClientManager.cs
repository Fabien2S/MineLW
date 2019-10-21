using System.Collections.Generic;
using System.Numerics;
using MineLW.API;
using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Worlds;
using MineLW.Entities.Living.Player;
using MineLW.Worlds.Chunks.Generator;

namespace MineLW.Client
{
    public class ClientManager : IClientManager
    {
        private readonly IServer _server;

        private readonly ISet<IClient> _clients = new HashSet<IClient>();

        public ClientManager(IServer server)
        {
            _server = server;
        }

        public void Initialize(IClientConnection connection, PlayerProfile profile)
        {
            var client = new Client(connection, profile);
            
            var player = new EntityPlayer(0, client);

            var worldManager = _server.WorldManager;
            var defaultWorld = worldManager.CreateWorld(worldManager.DefaultWorld);

            var blockManager = _server.BlockManager;
            var stoneBlock = blockManager[Minecraft.Blocks.Stone];
            var stoneBlockState = stoneBlock.CreateDefaultState();
            
            defaultWorld.ChunkManager.Generator = new DefaultChunkGenerator(stoneBlockState);
            defaultWorld.SetOption(WorldOption.SpawnPosition, Vector3.UnitY);
            
            player.WorldContext = defaultWorld;
            player.Position = defaultWorld.GetOption(WorldOption.SpawnPosition);
            player.Rotation = defaultWorld.GetOption(WorldOption.SpawnRotation);

            client.Init(player);
            _clients.Add(client);
        }

        public void Update(float deltaTime)
        {
            foreach (var client in _clients)
                client.Update(deltaTime);
        }
    }
}