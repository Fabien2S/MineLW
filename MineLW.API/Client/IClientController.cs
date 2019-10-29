using System;
using System.Numerics;
using MineLW.API.Math;
using MineLW.API.Text;

namespace MineLW.API.Client
{
    public interface IClientController
    {
        IClient Client { get; set; }

        event EventHandler<TextComponent> Disconnected;  
            
        event EventHandler<Vector3> PositionChanged;
        event EventHandler<Rotation> RotationChanged;
        event EventHandler<int> TeleportConfirmed;
        event EventHandler<long> PingResponseReceived;
    }
}