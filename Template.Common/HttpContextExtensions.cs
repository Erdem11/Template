using System.Linq;
using Microsoft.AspNetCore.Http;
using Template.Common.Structs;

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
    }
}