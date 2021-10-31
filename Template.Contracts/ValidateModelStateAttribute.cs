using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Template.Common;
using Template.Contracts.V1;
using Template.Localization;

namespace Template.Contracts
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Custom response on validation error
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            var errors = context.ModelState.Where(x => x.Value.Errors.Count > 0)
                .Select(x => new ValidationError
                {
                    Key = x.Key,
                    ErrorMessages = x.Value.Errors.Select(xx => xx.ErrorMessage.ToString()).ToList()
                }).ToList();

            var acceptLanguage = context.HttpContext.Request.Headers[HttpConstants.AcceptLanguage].ToString();

            var language = StringHelpers.AcceptLanguageToLanguage(acceptLanguage);
            foreach (var error in errors)
            {
                for (var i = 0; i < error.ErrorMessages.Count; i++)
                {
                    error.ErrorMessages[i] = error.ErrorMessages[i].Replace(error.Key, error.Key.Localize(language));
                }
            }

            var responseObj = new ValidationErrorResponse
            {
                Message = HttpConstants.BadRequest ,
                Errors = errors
            };

            context.Result = new JsonResult(responseObj)
            {
                StatusCode = 400
            };
        }
    }
}