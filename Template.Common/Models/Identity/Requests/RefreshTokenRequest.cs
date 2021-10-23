using System;
using Template.Common.Models.ModelBase;

namespace Template.Common.Models.Identity.Requests
{
    public class RefreshTokenRequest : IRequest
    {
        public string Token { get; set; }
        public Guid RefreshToken { get; set; }
    }
}