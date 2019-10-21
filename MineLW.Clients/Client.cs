using MineLW.API.Client;
using MineLW.API.Client.World;
using MineLW.API.Entities.Living.Player;
using MineLW.Clients.World;

namespace MineLW.Clients
{
    public class Client : IClient
    {
        public PlayerProfile Profile { get; }
        public IClientConnection Connection { get; }
        
        public IEntityPlayer Player { get; private set; }
        public IClientWorld World { get; }

        public Client(IClientConnection connection, PlayerProfile profile)
        {
            Profile = profile;
            Connection = connection;
            World = new ClientWorld(this);
        }
        
        public void Init(IEntityPlayer player)
        {
            if(Player != null)
            {
                Connection.Disconnect();
                return;
            }
            
            Player = player;
            World.Init();
            Connection.JoinGame(this, player);
        }

        public void Update(float deltaTime)
        {
            World.Update(deltaTime);
        }
    }
}