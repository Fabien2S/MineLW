using System;
using MineLW.API.Client;
using MineLW.API.Client.World;
using MineLW.API.Entities.Events;
using MineLW.API.Worlds;
using MineLW.API.Worlds.Chunks;

namespace MineLW.Client.World
{
    public class ClientWorld : IClientWorld
    {
        private const byte DefaultRenderDistance = 10;

        public IClientChunkManager ChunkManager { get; }
        public IClientEntityManager EntityManager { get; }

        public ChunkPosition ChunkPosition
        {
            get => _chunkPosition;
            set
            {
                _chunkPosition = value;
                _worldDirty = true;
            }
        }

        public byte RenderDistance
        {
            get => _renderDistance;
            set
            {
                var tmp = Math.Clamp(value, (byte) 0, DefaultRenderDistance);
                if (_renderDistance == tmp)
                    return;
                
                _renderDistance = tmp;
                _worldDirty = true;
            }
        }

        private readonly IClient _client;

        private ChunkPosition _chunkPosition;
        private byte _renderDistance = DefaultRenderDistance;
        
        private bool _worldDirty;

        public ClientWorld(IClient client)
        {
            _client = client;
            ChunkManager = new ClientChunkManager(_client);
            EntityManager = new ClientEntityManager();
        }

        public void Init()
        {
            _client.Player.PositionChanged += OnPlayerPositionChanged;
            _client.Player.WorldChanged += OnPlayerWorldChanged;
            
            _worldDirty = true;
        }

        public void Update(float deltaTime)
        {
            if (!_worldDirty)
                return;
            
            _worldDirty = false;
            
            ChunkManager.SynchronizeChunks();
            EntityManager.SynchronizeEntities();
        }

        private void OnPlayerPositionChanged(object sender, EntityPositionChangedEventArgs e)
        {
            var playerChunk = ChunkPosition.FromWorld(e.To);
            if (ChunkPosition == playerChunk)
                return;
            
            ChunkPosition = playerChunk;
            _worldDirty = true;
        }
        
        private void OnPlayerWorldChanged(object sender, EntityWorldChangedEventArgs e)
        {
            if (!DoesWorldRequireReload(e.From, e.To))
                return;

            if (e.From != null)
                _client.Connection.Respawn(e.To);

            _worldDirty = true;
        }

        private static bool DoesWorldRequireReload(IWorldContext from, IWorldContext to)
        {
            if (from == null)
                return true;
            
            var currentEnvironment = from.GetOption(WorldOption.Environment);
            var destinationEnvironment = to.GetOption(WorldOption.Environment);
            if (currentEnvironment != destinationEnvironment)
                return true;

            var currentWorld = from.World;
            var destinationWorld = to.World;
            return currentWorld != destinationWorld;
        }
    }
}