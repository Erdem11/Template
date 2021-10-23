using System;
using Microsoft.AspNetCore.Mvc;
using Template.Common.Models.Identity.Requests;
using Template.Common.Models.Identity.Responses;
using Template.Common.Models.ModelBase;
using Template.Service;

namespace Template.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost]
        public AuthResponse Register(RegisterRequest request)
        {
            return _identityService.Register(request);
        }

        [HttpPost]
        public AuthResponse Login(LoginRequest request)
        {
            return _identityService.Login(request);
        }

        [HttpPost]
        public AuthResponse RefreshToken(RefreshTokenRequest request)
        {
            return _identityService.RefreshToken(request);
        }

        [HttpPost]
        public EmptyResponse AddUserClaim(Guid userId, string claimName)
        {
            var result = _identityService.AddUserClaim(userId, claimName);
            return result;
        }
    }
}