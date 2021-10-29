using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Template.Common;

namespace Template.Api.Controllers
{
    public class TemplateControllerBase : Controller
    {
        private Guid? _contextUserId;
        protected Guid ContextUserId => _contextUserId ??= (Guid)HttpContext.GetUserId().GetValueOrDefault();

        private bool? _isAccessTokenExists;
        private string _accessToken;

        protected string AccessToken
        {
            get
            {
                switch (_isAccessTokenExists)
                {
                    case true:
                        return _accessToken;
                    case false:
                        return null;
                }

                var possibleToken = HttpContext.Request.Headers["Authorization"].ToString();

                if (possibleToken.StartsWith("Bearer "))
                {
                    _accessToken ??= possibleToken["Bearer ".Length..];
                }
                
                _isAccessTokenExists = _accessToken != null;
                return _accessToken;
            }
        }

        protected Task<string> AccessTokenTask() => Task.FromResult(AccessToken);
    }
}