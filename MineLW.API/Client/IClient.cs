using System;
using DotNetty.Buffers;
using MineLW.API.Client.World;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Utils;

namespace MineLW.API.Client
{
    public interface IClient : IUpdatable
    {
        /// <summary>
        /// Gets the client profile. This is the profile that was received from Mojang auth servers
        /// </summary>
        PlayerProfile Profile { get; }
        /// <summary>
        /// <para>Gets the client connection. This is responsible for sending data to the client.</para>
        /// This should never be used by a plugin
        /// </summary>
        IClientConnection Connection { get; }
        /// <summary>
        /// <para>This is responsible for receiving client's data.</para>
        /// Any plugin is free to subscribe to any events, however, they are probably not reliable across versions
        /// </summary>
        IClientController Controller { get; }

        IEntityPlayer Player { get; }
        IClientWorld World { get; }
        
        float Latency { get; }

        void Init(IEntityPlayer player);
        void SendCustom(Identifier channel, Action<IByteBuffer> serializer);
    }
}