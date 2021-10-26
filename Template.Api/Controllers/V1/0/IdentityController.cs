using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Common;
using Template.Contracts.V1;
using Template.Contracts.V1.Identity.Requests;
using Template.Contracts.V1.Identity.Responses;
using Template.Service;

namespace Template.Api.Controllers.V1._0
{
    [ApiController]
    [Route("[controller]/[action]")]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class IdentityController : TemplateControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public IdentityController(IIdentityService identityService, IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Response<AuthResponse>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public IActionResult Register(RegisterRequest request)
        {
            var result = _identityService.Register(request.Email, request.Password);
            if (result.HasError)
            {
                return BadRequest(new ErrorResponse(result.Error));
            }

            var response = _mapper.Map<AuthResponse>(result);

            return Ok(new Response<AuthResponse>(response));
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Response<AuthResponse>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
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
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Response<AuthResponse>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public IActionResult RefreshToken(RefreshTokenRequest request)
        {
            var result = _identityService.RefreshToken(request.Token, request.RefreshToken);
            if (result.HasError)
            {
                return BadRequest(new ErrorResponse(result.Error));
            }

            var response = _mapper.Map<AuthResponse>(result);

            return Ok(new Response<AuthResponse>(response));
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.Admin)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(OkResponse), 200)]
        public IActionResult AddUserClaim(Guid userId, string claimName)
        {
            _identityService.AddUserClaim(userId, claimName);

            return Ok(new OkResponse(true));
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.Admin)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(OkResponse), 200)]
        public IActionResult AddRole(string role)
        {
            _identityService.AddRole(role);

            return Ok(new OkResponse(true));
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.Admin)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(OkResponse), 200)]
        public IActionResult AddUserRole(Guid id, string role)
        {
            _identityService.AddUserRole(id, role);

            return Ok(new OkResponse(true));
        }

        [HttpGet]
        [Authorize(Roles = RoleConstants.Admin)]
        [MapToApiVersion("1.0")]
        [ProducesErrorResponseType(typeof(void))]
        [ProducesResponseType(typeof(UnauthorizedResult), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public IActionResult AdminTest()
        {
            return NoContent();
        }
    }
}