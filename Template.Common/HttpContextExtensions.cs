using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Template.Common.Types;

namespace Template.Common
{
    public static class HttpContextExtensions
    {
        public static Guid? GetUserId(this HttpContext httpContext)
        {
            if (httpContext?.User == default)
                return default;

            var idString = httpContext.User.Claims.Single(x => x.Type == "id").Value;
            _ = Guid.TryParse(idString, out var id);
            return id;
        }

        public static string GetToken(this HttpContext httpContext)
        {
            var possibleToken = httpContext.Request.Headers[HttpConstants.Authorization].ToString();

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
            var possibleLanguageString = httpContext.Request.Headers[HttpConstants.AcceptLanguage].ToString();

            return StringHelpers.AcceptLanguageToLanguage(possibleLanguageString);
        }
    }
}