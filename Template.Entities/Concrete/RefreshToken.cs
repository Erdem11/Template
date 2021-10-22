using System;
using Template.Entities.Abstract;
using Template.Entities.Concrete.IdentityModels;

namespace Template.Entities.Concrete
{
    public class RefreshToken : EntityBase
    {
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Used { get; set; }
        public bool Invalidated { get; set; }
        public User User { get; set; }
        public MyKey UserId { get; set; }
    }
}