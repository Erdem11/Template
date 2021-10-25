using System;

namespace Template.Contracts.V1.Identity.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime TokenExpireDate { get; set; }
        public Guid RefreshToken { get; set; }
        public string Error { get; set; }
    }
}