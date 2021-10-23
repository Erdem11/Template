using System;
using Template.Entities.Abstract;

namespace Template.Entities.Concrete
{
    public class Author : EntityBase, ISoftDelete
    {
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}