using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Template.Common;
using Template.Common.Models.Identity.Requests;
using Template.Common.Models.Identity.Responses;
using Template.Common.Models.ModelBase;
using Template.Common.Structs;
using Template.Data;
using Template.Entities.Concrete;
using Template.Entities.Concrete.IdentityModels;

namespace Template.Service
{
    public interface IIdentityService
    {
        AuthResponse Register(RegisterRequest request);
        EmptyResponse AddUserClaim(Guid userId, string claimName);
        AuthResponse Login(LoginRequest request);
        AuthResponse RefreshToken(RefreshTokenRequest request);
        EmptyResponse AddUserRole(MyKey userId, string role);
        EmptyResponse AddRole(string role);
    }

    public class IdentityService : IIdentityService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly TemplateContext _templateContext;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public IdentityService(UserManager<User> userManager,RoleManager<Role> roleManager, JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, TemplateContext templateContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _templateContext = templateContext;
        }

        public AuthResponse Register(RegisterRequest request)
        {
            var newUser = new User
            {
                Email = request.Email,
                UserName = request.Email
            };

            var identityResult = _userManager.CreateAsync(newUser, request.Password).Result;

            if (!identityResult.Succeeded)
                return default;

            var user = _userManager.FindByEmailAsync(request.Email).Result;
            
            return GenerateAuthenticationResultForUser(user);
        }

        public AuthResponse Login(LoginRequest request)
        {
            var user = _userManager.FindByEmailAsync(request.Email).Result;

            if (user == default)
                return ResponseBase.ErrorResponse<AuthResponse>("User does not exist");

            var isPasswordValid = _userManager.CheckPasswordAsync(user, request.Password).Result;

            if (!isPasswordValid)
                return ResponseBase.ErrorResponse<AuthResponse>("User/password combination is wrong");

            return GenerateAuthenticationResultForUser(user);
        }

        public EmptyResponse AddRole(string role)
        {
            _roleManager.CreateAsync(new Role
            {
                Name = role
            }).Wait();
            
            return EmptyResponse.Create();
        }

        public EmptyResponse AddUserRole(MyKey userId, string role)
        {
            var user = _templateContext.Users.Find(userId);

            _userManager.AddToRoleAsync(user, role).Wait();

            return EmptyResponse.Create();
        }
        
        public EmptyResponse AddUserClaim(Guid userId, string claimName)
        {
            var user = _userManager.FindByIdAsync(userId.ToString()).Result;
            _userManager.AddClaimAsync(user, new Claim(claimName, "true")).Wait();
            _templateContext.SaveChanges();

            return EmptyResponse.Create();
        }

        public AuthResponse RefreshToken(RefreshTokenRequest request)
        {
            var response = new AuthResponse();
            var validatedToken = GetPrincipalFromToken(request.Token);

            if (validatedToken == null)
                // invalid token
                return default;

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = DateTime.UnixEpoch.AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                // this token hasn't expired yet
                response.Error = "This token has not expired yet";
                return response;
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value;

            var storedRefreshToken = _templateContext.RefreshTokens.SingleOrDefault(x => x.Id == request.RefreshToken);

            if (storedRefreshToken == default)
            {
                // this refresh token doesn't exist
                response.Error = "this refresh token doesn't exist";
                return response;
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                // this refresh token has expired
                response.Error = "this refresh token has expired";
                return response;
            }

            if (storedRefreshToken.Invalidated)
            {
                // this refresh token has been Invalidated
                response.Error = "this refresh token has been Invalidated";
                return response;
            }

            if (storedRefreshToken.Used)
            {
                // this refresh token has been Used
                response.Error = "this refresh token has been Used";
                return response;
            }

            if (storedRefreshToken.JwtId != jti)
            {
                // this refresh does not match this JWT
                response.Error = "this refresh does not match this JWT";
                return response;
            }

            storedRefreshToken.Used = true;
            _templateContext.RefreshTokens.Update(storedRefreshToken);
            _templateContext.SaveChanges();

            var user = _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value).Result;

            return GenerateAuthenticationResultForUser(user);
        }

        private AuthResponse GenerateAuthenticationResultForUser(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("id", user.Id.ToString())
            };

            var userClaims = _userManager.GetClaimsAsync(user).Result;
            claims.AddRange(userClaims);

            var userRoles = _userManager.GetRolesAsync(user).Result;
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.Add(_jwtSettings.RefreshTokenLifeTime)
            };

            _templateContext.RefreshTokens.Add(refreshToken);
            _templateContext.SaveChanges();

            var tokenString = tokenHandler.WriteToken(token);

            return new AuthResponse
            {
                Token = tokenString,
                TokenExpireDate = tokenDescriptor.Expires.GetValueOrDefault(),
                RefreshToken = refreshToken.Id.ToPrimitive()
            };
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                    return null;
                    
                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return validatedToken is JwtSecurityToken jwtSecurityToken &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}