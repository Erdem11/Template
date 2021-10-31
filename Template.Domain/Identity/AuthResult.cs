using System;

namespace Template.Domain.Identity
{
    public class AuthResult
    {
        public string Token { get; set; }
        public DateTime TokenExpireDate { get; set; }
        public Guid RefreshToken { get; set; }
        public bool HasError => !string.IsNullOrWhiteSpace(Error);
        public string Error { get; set; }
    }
}