using System;
using MineLW.API.Entities;
using MineLW.API.Worlds.Context;

namespace MineLW.Entities
{
    public class Entity : IEntity
    {
        public int Id { get; }
        public Guid Uuid { get; }
        public IWorldContext Context { get; set; }
    }
}