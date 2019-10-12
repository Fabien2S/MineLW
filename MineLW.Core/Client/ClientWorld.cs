using MineLW.API.Client;
using MineLW.API.Entities.Events;
using MineLW.API.Math;
using MineLW.API.Worlds;

namespace MineLW.Client
{
    public class ClientWorld : IClientWorld
    {
        public const byte DefaultRenderDistance = 10;

        public Vector2Int ChunkPosition { get; }
        public byte RenderDistance { get; set; } = DefaultRenderDistance;

        private readonly IClient _client;

        public ClientWorld(IClient client)
        {
            _client = client;
        }

        public void Init()
        {
            _client.Player.WorldChanged += OnPlayerWorldChanged;
        }
        
        private void OnPlayerWorldChanged(object sender, EntityWorldEventArgs e)
        {
            if (!DoesWorldRequireReload(e.From, e.To))
                return;

            if (e.From != null)
                _client.Respawn(e.To);
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