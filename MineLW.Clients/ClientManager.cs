using System.Collections.Generic;
using MineLW.API;
using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Worlds;
using MineLW.Entities.Living.Player;

namespace MineLW.Clients
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

            var worldManager = _server.WorldManager;
            var defaultWorld = worldManager[worldManager.DefaultWorld];
            var player = new EntityPlayer(0, client)
            {
                WorldContext = defaultWorld,
                Position = defaultWorld.GetOption(WorldOption.SpawnPosition),
                Rotation = defaultWorld.GetOption(WorldOption.SpawnRotation)
            };

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