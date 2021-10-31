using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Template.Common.Structs;
using Template.Common.Types;

namespace Template.Common
{
    [ApiController]
    [Route("[controller]/[action]")]
    [TypeFilter(typeof(TemplateControllerBaseAttribute))]
    public class TemplateControllerBase : Controller
    {
        private Guid? _contextUserId;
        protected Guid ContextUserId => _contextUserId ??= HttpContext.GetUserId().GetValueOrDefault();

        private bool? _isAccessTokenExist;
        private string _accessToken;

        protected string AccessToken
        {
            get
            {
                switch (_isAccessTokenExist)
                {
                    case true:
                        return _accessToken;
                    case false:
                        return null;
                }

                _accessToken = HttpContext.GetToken();

                _isAccessTokenExist = _accessToken != null;
                return _accessToken;
            }
        }

        private bool? _isTokenModelExist;
        private TokenModel _tokenModel;

        protected TokenModel TokenModel
        {
            get
            {
                switch (_isTokenModelExist)
                {
                    case true:
                        return _tokenModel;
                    case false:
                        return null;
                }

                _tokenModel = HttpContext.GetTokenModel();
                _isTokenModelExist = _tokenModel != null;

                return _tokenModel;
            }
        }

        private bool _isLanguageSet;
        private Languages _language;

        protected Languages Language
        {
            get
            {
                if (_isLanguageSet)
                {
                    return _language;
                }

                _language = HttpContext.GetLanguage();
                _isLanguageSet = true;

                return _language;
            }
        }

        protected Task<string> AccessTokenTask() => Task.FromResult(AccessToken);
    }

    public class TemplateControllerBaseAttribute : ActionFilterAttribute
    {
        private readonly LocalizationInfo _localizationInfo;
   
        public TemplateControllerBaseAttribute(LocalizationInfo localizationInfo)
        {
            _localizationInfo = localizationInfo;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _localizationInfo.Language = context.HttpContext.GetLanguage();
        }
    }
}