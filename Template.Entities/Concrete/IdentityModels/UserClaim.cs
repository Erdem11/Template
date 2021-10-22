using System;
using Microsoft.AspNetCore.Identity;
using Template.Entities.Abstract;

namespace Template.Entities.Concrete.IdentityModels
{
    public class UserClaim : IdentityUserClaim<MyKey>, IEntityBase
    {
        public new MyKey Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}