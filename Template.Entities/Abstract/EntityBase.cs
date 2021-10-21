using System;

namespace Template.Entities.Abstract
{
    public interface IEntityBase
    {
        Guid Id { get; set; }
        DateTime CreatedAt { get; set; }
    }
    
    public class EntityBase : IEntityBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}