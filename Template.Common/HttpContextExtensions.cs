using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Template.Common
{
    public static class HttpContextExtensions
    {
        public static Guid? GetUserId(this HttpContext httpContext)
        {
            if (httpContext?.User == default)
            {
                return default;
            }

            var idString = httpContext.User.Claims.Single(x => x.Type == "id").Value;
            
            return Guid.Parse(idString);
        }
    }
}