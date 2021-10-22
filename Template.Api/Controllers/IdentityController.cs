using System;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public object Register(string email, string password)
        {
            return _identityService.Register(email, password);
        }

        [HttpPost]
        public object AddUserClaim(Guid userId, string claimName)
        {
            _identityService.AddUserClaim(userId, claimName);
            return default;
        }
    }
}