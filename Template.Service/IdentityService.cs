using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Template.Common;
using Template.Data;
using Template.Entities.Concrete;

namespace Template.Service
{
    public interface IIdentityService
    {
        string Register(string email, string password);
        void AddUserClaim(Guid userId, string claimName);
    }

    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly TemplateContext _templateContext;

        public IdentityService(UserManager<User> userManager, JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, TemplateContext templateContext)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _templateContext = templateContext;
        }

        public string Register(string email, string password)
        {
            var newUser = new User
            {
                Email = email,
                UserName = email,
            };

            var createdUser = _userManager.CreateAsync(newUser, password).Result;

            if (!createdUser.Succeeded)
            {
                return default;
            }

            return GenerateAuthenticationResultForUser(newUser);
        }
        
        public void AddUserClaim(Guid userId, string claimName)
        {
            var user = _userManager.FindByIdAsync(userId.ToString()).Result;
            _userManager.AddClaimAsync(user, new Claim(claimName, "true"));
            _templateContext.SaveChanges();
        }
        private string GenerateAuthenticationResultForUser(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id.ToString()),
            };

            var userClaims = _userManager.GetClaimsAsync(user).Result;
            claims.AddRange(userClaims);
            
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
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
            };

            _templateContext.RefreshTokens.Add(refreshToken);
            _templateContext.SaveChanges();

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public string RefreshToken(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
            {
                // invalid token
                return default;
            }

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                // this token hasn't expired yet
                return default;
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value;

            var storedRefreshToken = _templateContext.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

            if (storedRefreshToken == default)
            {
                // this refresh token doesn't exist
                return default;
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                // this refresh token has expired
                return default;
            }

            if (storedRefreshToken.Invalidated)
            {
                // this refresh token has been Invalidated
                return default;
            }

            if (storedRefreshToken.Used)
            {
                // this refresh token has been Used
                return default;
            }

            if (storedRefreshToken.JwtId != jti)
            {
                // this refresh does not match this JWT
                return default;
            }

            storedRefreshToken.Used = true;
            _templateContext.RefreshTokens.Update(storedRefreshToken);
            _templateContext.SaveChanges();

            var user = _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value).Result;

            return GenerateAuthenticationResultForUser(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}