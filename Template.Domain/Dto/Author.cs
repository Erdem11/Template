using System;
using Template.Domain.Dto.Abstract;

namespace Template.Domain.Dto
{
    public class Author : EntityBase, ISoftDelete
    {
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}