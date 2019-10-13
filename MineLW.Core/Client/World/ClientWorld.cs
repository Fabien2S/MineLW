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
                _renderDistance = value;
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
            _client.Player.WorldChanged += OnPlayerWorldChanged;
            _worldDirty = true;
        }

        public void Update(float deltaTime)
        {
            if (_worldDirty)
            {
                // TODO synchronize chunks and entities
            }
        }
        
        private void OnPlayerWorldChanged(object sender, EntityWorldChangedEventArgs e)
        {
            if (!DoesWorldRequireReload(e.From, e.To))
                return;

            if (e.From != null)
                _client.Connection.Respawn(e.To);
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