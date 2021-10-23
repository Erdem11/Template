using System;
using Microsoft.AspNetCore.Identity;
using Template.Common.Structs;
using Template.Entities.Abstract;

namespace Template.Entities.Concrete.IdentityModels
{
    public class User : IdentityUser<MyKey>, IEntityBase
    {
        public new MyKey Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}