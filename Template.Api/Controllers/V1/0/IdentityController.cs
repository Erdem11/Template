using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Contracts.V1;
using Template.Contracts.V1.Identity.Requests;
using Template.Contracts.V1.Identity.Responses;
using Template.Service;

namespace Template.Api.Controllers.V1._0
{
    [ApiController]
    [Route("[controller]/[action]")]
    [ApiVersion( "1.0" )]
    [ApiVersion( "1.1" )]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [MapToApiVersion("1.0")]
        public IActionResult Register(RegisterRequest request)
        {
            var result = _identityService.Register(request.Email, request.Password);
            if (result.HasError)
            {
                return BadRequest(new ErrorResponse(result.Error));
            }

            return Ok(new AuthResponse()
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken,
                TokenExpireDate = result.TokenExpireDate
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response<AuthResponse>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [MapToApiVersion("1.0")]
        public IActionResult Login(LoginRequest request)
        {
            var result = _identityService.Login(request.Email, request.Password);

            if (result.HasError)
            {
                return BadRequest(new ErrorResponse(result.Error));
            }

            return Ok(new Response<AuthResponse>(new AuthResponse
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken,
                TokenExpireDate = result.TokenExpireDate
            }));
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [MapToApiVersion("1.0")]
        public IActionResult RefreshToken(RefreshTokenRequest request)
        {
            var result = _identityService.RefreshToken(request.Token, request.RefreshToken);
            if (result.HasError)
            {
                return BadRequest(new ErrorResponse(result.Error));
            }

            return Ok(new AuthResponse()
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken,
                TokenExpireDate = result.TokenExpireDate
            });
        }

        // [HttpPost]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        // [ProducesResponseType(typeof(ErrorResponse), 400)]
        // public IActionResult AddUserClaim(Guid userId, string claimName)
        // {
        //     var result = _identityService.AddUserClaim(userId, claimName);
        //     return result;
        // }
        //
        // [HttpPost]
        // [ProducesResponseType(typeof(ErrorResponse), 400)]
        // public IActionResult AddRole(string role)
        // {
        //     _identityService.AddRole(role);
        //
        //     return EmptyResponse.Create();
        // }
        //
        // [HttpPost]
        // [ProducesResponseType(typeof(ErrorResponse), 400)]
        // public IActionResult AddUserRole(MyKey id, string role)
        // {
        //     _identityService.AddUserRole(id, role);
        //
        //     return EmptyResponse.Create();
        // }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        // [ProducesDefaultResponseType]
        [ProducesErrorResponseType(typeof(void))]
        // [ProducesResponseType(typeof(NoContentResult), 204)]
        [ProducesResponseType(typeof(UnauthorizedResult), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [MapToApiVersion("1.0")]
        public IActionResult AdminTest()
        {
            return NoContent();

            // return EmptyResponse.Create();
        }
    }
}