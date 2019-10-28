using System.Collections.Generic;
using MineLW.API;
using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Server;
using MineLW.API.Worlds;
using MineLW.Networking.IO;

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

        public void Initialize(IClientConnection connection, IClientController controller, PlayerProfile profile)
        {
            var client = new Client(connection, controller, profile);
            
            var worldManager = _server.WorldManager;
            var world = worldManager[worldManager.DefaultWorld];

            var entityManager = world.EntityManager;
            var player = entityManager.SpawnPlayer(
                client,
                world.GetOption(WorldOption.SpawnPosition),
                world.GetOption(WorldOption.SpawnRotation)
            );

            client.Init(player);
            client.SendCustom(Minecraft.Channels.Brand, buffer => { buffer.WriteUtf8(_server.Name); });
            
            _clients.Add(client);
            
            // TODO Implements client tp (with "waiting for confirm" state)
            client.Connection.Teleport(player.Position, player.Rotation, 0);
        }

        public void Update(float deltaTime)
        {
            foreach (var client in _clients)
                client.Update(deltaTime);
        }
    }
}