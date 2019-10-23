using System.Collections.Generic;
using MineLW.API;
using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Server;
using MineLW.API.Worlds;
using MineLW.Entities.Living.Player;
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
            var defaultWorld = worldManager[worldManager.DefaultWorld];
            var player = new EntityPlayer(0, client)
            {
                WorldContext = defaultWorld,
                Position = defaultWorld.GetOption(WorldOption.SpawnPosition),
                Rotation = defaultWorld.GetOption(WorldOption.SpawnRotation)
            };

            client.Init(player);
            controller.Init(player);
            
            client.SendCustom(Minecraft.Channels.Brand, buffer => { buffer.WriteUtf8(_server.Name); });
            
            _clients.Add(client);
            
            client.Connection.Teleport(player.Position, player.Rotation, 0);
        }

        public void Update(float deltaTime)
        {
            foreach (var client in _clients)
                client.Update(deltaTime);
        }
    }
}