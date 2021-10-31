using System;
using Template.Common.Structs;
using Template.Domain.Dto.Abstract;
using Template.Domain.Dto.IdentityModels;

namespace Template.Domain.Dto
{
    public class RefreshToken : EntityBase
    {
        public string JwtId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Used { get; set; }
        public bool Invalidated { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}