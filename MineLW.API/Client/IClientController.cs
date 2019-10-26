using System;
using System.Numerics;

namespace MineLW.API.Client
{
    public interface IClientController
    {
        IClient Client { get; set; }

        event EventHandler<Vector3> PositionChanged;
        event EventHandler<int> TeleportConfirmed;
        event EventHandler<long> PingResponseReceived;
    }
}