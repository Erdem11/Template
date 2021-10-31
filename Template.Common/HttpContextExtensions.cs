using System.Linq;
using Microsoft.AspNetCore.Http;
using Template.Common.Structs;
using Template.Common.Types;

namespace Template.Common
{
    public static class HttpContextExtensions
    {
        public static MyKey? GetUserId(this HttpContext httpContext)
        {
            if (httpContext?.User == default)
                return default;

            var idString = httpContext.User.Claims.Single(x => x.Type == "id").Value;

            return MyKey.Parse(idString);
        }

        public static string GetToken(this HttpContext httpContext)
        {
            var possibleToken = httpContext.Request.Headers["Authorization"].ToString();

            return possibleToken.StartsWith("Bearer ") ? possibleToken["Bearer ".Length..] : default;
        }

        public static TokenModel GetTokenModel(this HttpContext httpContext)
        {
            var isTokenModelExists = httpContext.Items.TryGetValue(nameof(TokenModel), out var tokenModelObj);
            if (isTokenModelExists && tokenModelObj is TokenModel tokenModel)
            {
                return tokenModel;
            }
            return default;
        }

        public static Languages GetLanguage(this HttpContext httpContext)
        {
            var possibleLanguageString = httpContext.Request.Headers["Accept-Language"].ToString();

            return StringHelpers.AcceptLanguageToLanguage(possibleLanguageString);
        }
    }
}