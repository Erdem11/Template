using System;
using Microsoft.AspNetCore.Identity;
using Template.Entities.Abstract;

namespace Template.Entities.Concrete
{
    public class User : IdentityUser<Guid>, IEntityBase
    {
        public DateTime CreatedAt { get; set; }
    }
}