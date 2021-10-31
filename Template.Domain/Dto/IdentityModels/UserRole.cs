using System;
using Microsoft.AspNetCore.Identity;
using Template.Common.Structs;
using Template.Domain.Dto.Abstract;

namespace Template.Domain.Dto.IdentityModels
{
    public class UserRole : IdentityUserRole<Guid>, ICreatedAt
    {
        public DateTime CreatedAt { get; set; }
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}