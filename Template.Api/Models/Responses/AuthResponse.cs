using System;
using Template.Api.Models.ModelBase;

namespace Template.Api.Models.Responses
{
    public class AuthResponse : ResponseBase<AuthResponse>, IResponse 
    {
        public string Token { get; set; }
        public DateTime TokenExpireDate { get; set; }
        public Guid RefreshToken { get; set; }
    }
}