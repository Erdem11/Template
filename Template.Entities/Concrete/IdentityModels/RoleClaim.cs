using System;
using Microsoft.AspNetCore.Identity;
using Template.Entities.Abstract;

namespace Template.Entities.Concrete.IdentityModels
{
    public class RoleClaim : IdentityRoleClaim<MyKey>, IEntityBase
    {
        public new MyKey Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}