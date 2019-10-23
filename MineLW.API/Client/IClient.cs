using System;
using DotNetty.Buffers;
using MineLW.API.Client.World;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Utils;

namespace MineLW.API.Client
{
    public interface IClient : IUpdatable
    {
        PlayerProfile Profile { get; }
        IClientConnection Connection { get; }
        IClientController Controller { get; }

        IEntityPlayer Player { get; }
        IClientWorld World { get; }

        void Init(IEntityPlayer player);
        void SendCustom(Identifier channel, Action<IByteBuffer> serializer);
    }
}