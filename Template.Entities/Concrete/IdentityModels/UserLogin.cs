using System;
using Microsoft.AspNetCore.Identity;
using Template.Entities.Abstract;

namespace Template.Entities.Concrete.IdentityModels
{
    public class UserLogin : IdentityUserLogin<MyKey>, IEntityBase
    {
        public MyKey Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}