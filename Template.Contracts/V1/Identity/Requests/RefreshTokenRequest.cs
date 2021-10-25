using System;
using Template.Contracts.V1.ModelBase;

namespace Template.Contracts.V1.Identity.Requests
{
    public class RefreshTokenRequest : IRequest
    {
        public string Token { get; set; }
        public Guid RefreshToken { get; set; }
    }
}