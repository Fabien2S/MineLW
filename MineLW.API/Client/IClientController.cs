using System;
using MineLW.API.Entities.Events;
using MineLW.API.Entities.Living.Player;

namespace MineLW.API.Client
{
    public interface IClientController
    {
        event EventHandler<EntityPositionChangedEventArgs> PositionChanged;

        void Init(IEntityPlayer player);
    }
}