using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Template.Common;
using Template.Common.SettingsConfigurationFiles;
using Template.Common.Structs;
using Template.Common.Types;
using Template.Data;
using Template.Domain.Dto;
using Template.Domain.Dto.Abstract;
using Template.Domain.Dto.IdentityModels;
using Template.Domain.Identity;
using Template.Localization;

namespace Template.Service
{
    public interface IIdentityService
    {
        AuthResult Register(string email, string password);
        void AddUserClaim(Guid userId, string claimName);
        AuthResult Login(string email, string password);
        AuthResult RefreshToken(string token, Guid refreshToken);
        void AddUserRole(Guid userId, string role);
        void AddRole(string role);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }

    public class IdentityService : ServiceBase<IEntityBase>, IIdentityService
    {
        private readonly TemplateContext _templateContext;
        private readonly LocalizationInfo _localizationInfo;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SettingsHolder _settingsHolder;

        public IdentityService(UserManager<User> userManager, RoleManager<Role> roleManager, SettingsHolder settingsHolder, TokenValidationParameters tokenValidationParameters, TemplateContext templateContext, LocalizationInfo localizationInfo) : base(templateContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _settingsHolder = settingsHolder;
            _tokenValidationParameters = tokenValidationParameters;
            _templateContext = templateContext;
            _localizationInfo = localizationInfo;
        }

        public AuthResult Register(string email, string password)
        {
            var newUser = new User
            {
                Email = email,
                UserName = email
            };

            var identityResult = _userManager.CreateAsync(newUser, password).Result;

            if (!identityResult.Succeeded)
                return new AuthResult
                {
                    Error = identityResult.Errors.FirstOrDefault()?.Description
                };

            var user = _userManager.FindByEmailAsync(email).Result;

            return GenerateAuthenticationResultForUser(user);
        }

        public AuthResult Login(string email, string password)
        {
            var user = _userManager.FindByEmailAsync(email).Result;

            if (user == default)
                return new AuthResult
                {
                    Error = Localizer.Localize(x => x.ErrorUserNotExist, _localizationInfo.Language)
                };
            var isPasswordValid = _userManager.CheckPasswordAsync(user, password).Result;

            if (!isPasswordValid)
                return new AuthResult
                {
                    Error = Localizer.Localize(x => x.ErrorWrongUserNamePassword, _localizationInfo.Language)
                };

            return GenerateAuthenticationResultForUser(user);
        }

        public void AddRole(string role)
        {
            _roleManager.CreateAsync(new Role
            {
                Name = role
            }).Wait();
        }

        public void AddUserRole(Guid userId, string role)
        {
            var user = _templateContext.Users.Find(userId);

            _userManager.AddToRoleAsync(user, role).Wait();
        }

        public void AddUserClaim(Guid userId, string claimName)
        {
            var user = _userManager.FindByIdAsync(userId.ToString()).Result;
            _userManager.AddClaimAsync(user, new Claim(nameof(TokenModel.CustomClaims), claimName)).Wait();
            _templateContext.SaveChanges();
        }

        public AuthResult RefreshToken(string token, Guid refreshToken)
        {
            var response = new AuthResult();
            var validatedToken = GetPrincipalFromToken(token);

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

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = _templateContext.RefreshTokens.SingleOrDefault(x => x.Id == refreshToken);

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

        private AuthResult GenerateAuthenticationResultForUser(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settingsHolder.JwtSettings.Secret);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("id", user.Id.ToString())
            };

            var userClaims = _userManager.GetClaimsAsync(user).Result;
            claims.AddRange(userClaims);

            var userRoles = _templateContext.UserRoles.AsNoTracking().Where(x => x.UserId == user.Id);
            var roles = _templateContext.Roles.AsNoTracking().Where(x => userRoles.Any(xx => xx.RoleId == x.Id));

            var roleClaims = _templateContext.RoleClaims.AsNoTracking().Where(x => roles.Select(xx => xx.Id).Contains(x.RoleId));

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
            claims.AddRange(roleClaims.Select(x => new Claim(x.ClaimType, x.ClaimValue)));

            claims = claims.Select(x => new
                {
                    x.Type,
                    x.Value
                }).Distinct()
                .Select(x => new Claim(x.Type, x.Value))
                .ToList();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_settingsHolder.JwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.Add(_settingsHolder.JwtSettings.RefreshTokenLifeTime)
            };

            _templateContext.RefreshTokens.Add(refreshToken);
            _templateContext.SaveChanges();

            var tokenString = tokenHandler.WriteToken(token);

            return new AuthResult
            {
                Token = tokenString,
                TokenExpireDate = tokenDescriptor.Expires.GetValueOrDefault(),
                RefreshToken = refreshToken.Id
            };
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
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