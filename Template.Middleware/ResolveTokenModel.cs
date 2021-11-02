using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Template.Common;

namespace Template.Middleware
{
    public class ResolveTokenModel
    {
        private readonly RequestDelegate _next;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public ResolveTokenModel(RequestDelegate next, TokenValidationParameters tokenValidationParameters)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task Invoke(HttpContext context)
        {
            var tokenString = context.GetToken();
            if (!string.IsNullOrWhiteSpace(tokenString))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    var claims = tokenHandler.ValidateToken(tokenString, _tokenValidationParameters, out _).Claims.ToList();
                    var tokenModel = SolveTokenModel(claims);

                    context.Items.Add(nameof(TokenModel), tokenModel);
                }
                catch (SecurityTokenExpiredException)
                {
                }
            }

            await _next(context);
        }

        private static TokenModel SolveTokenModel(IReadOnlyCollection<Claim> claims)
        {
            if (claims == default)
            {
                return default;
            }
            
            var name = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var jtiString = claims.FirstOrDefault(x => x.Type == "jti")?.Value;
            _ = Guid.TryParse(jtiString, out var jti);

            var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var idString = claims.FirstOrDefault(x => x.Type == "id")?.Value;
            _ = Guid.TryParse(idString, out var id);

            var roles = claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();

            var customClaims = claims.Where(x => string.Equals(x.Type, nameof(TokenModel.CustomClaims), StringComparison.CurrentCultureIgnoreCase)).Select(x => x.Value).ToList();

            var nbfString = claims.FirstOrDefault(x => x.Type == "nbf")?.Value;
            _ = int.TryParse(nbfString, out var nbf);

            var expString = claims.FirstOrDefault(x => x.Type == "exp")?.Value;
            _ = int.TryParse(expString, out var exp);

            var iatString = claims.FirstOrDefault(x => x.Type == "iat")?.Value;
            _ = int.TryParse(iatString, out var iat);

            var tokenModel = new TokenModel
            {
                Email = email,
                Exp = exp,
                Iat = iat,
                Id = id,
                Jti = jti,
                Nbf = nbf,
                Roles = roles,
                Sub = name,
                CustomClaims = customClaims
            };

            return tokenModel;
        }
    }
}