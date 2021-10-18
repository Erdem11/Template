using System;

namespace Template.Entities.Abstract
{
    public class EntityBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}