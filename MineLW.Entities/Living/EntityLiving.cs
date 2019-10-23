using System;
using MineLW.API.Entities.Living;

namespace MineLW.Entities.Living
{
    public abstract class EntityLiving : Entity, IEntityLiving
    {
        protected EntityLiving(int id, Guid uuid) : base(id, uuid)
        {
        }
    }
}