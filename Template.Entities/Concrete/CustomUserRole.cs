using System;
using Microsoft.AspNetCore.Identity;
using Template.Entities.Abstract;

namespace Template.Entities.Concrete
{
    public class CustomUserRole : IdentityRole<Guid>, IEntityBase
    {
        public DateTime CreatedAt { get; set; }
    }
}