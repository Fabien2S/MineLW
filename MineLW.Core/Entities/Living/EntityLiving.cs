using System;
using MineLW.API.Entities.Living;

namespace MineLW.Entities.Living
{
    public class EntityLiving : Entity, IEntityLiving
    {
        public float Health { get; set; }

        public EntityLiving(int id, Guid uuid) : base(id, uuid)
        {
        }
    }
}