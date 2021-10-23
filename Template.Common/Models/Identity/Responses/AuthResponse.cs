using System;
using Template.Common.Models.ModelBase;

namespace Template.Common.Models.Identity.Responses
{
    public class AuthResponse : ResponseBase
    {
        public string Token { get; set; }
        public DateTime TokenExpireDate { get; set; }
        public Guid RefreshToken { get; set; }
    }
}