using System;
using MineLW.API.Worlds.Context;

namespace MineLW.API.Entities
{
    public interface IEntity
    {
        int Id { get; }
        Guid Uuid { get; }

        IWorldContext Context { get; set; }
    }
}