using System.Collections.Generic;
using MineLW.API;
using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Worlds;
using MineLW.Entities.Living.Player;

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
            var spawnPosition = defaultWorld.GetOption(WorldOption.SpawnPosition);
            var spawnRotation = defaultWorld.GetOption(WorldOption.SpawnRotation);
            player.Move(defaultWorld, spawnPosition, spawnRotation);

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