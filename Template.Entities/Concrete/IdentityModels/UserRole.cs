using System;
using Microsoft.AspNetCore.Identity;
using Template.Common.Structs;
using Template.Entities.Abstract;

namespace Template.Entities.Concrete.IdentityModels
{
    public class UserRole : IdentityUserRole<MyKey>, ICreatedAt
    {
        public DateTime CreatedAt { get; set; }
    }
}