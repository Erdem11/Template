using System;
using Microsoft.AspNetCore.Identity;
using Template.Common.Structs;
using Template.Domain.Dto.Abstract;

namespace Template.Domain.Dto.IdentityModels
{
    public class UserLogin : IdentityUserLogin<Guid>, IEntityBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}