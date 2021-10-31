using System.Globalization;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Template.Common;

namespace Template.Contracts
{
    public class ValidatorInterceptor : IValidatorInterceptor
    {
        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            var acceptLanguage = actionContext.HttpContext.Request.Headers[HttpConstants.AcceptLanguage].ToString();
            if (string.IsNullOrWhiteSpace(acceptLanguage))
                return commonContext;

            var cultureString = acceptLanguage.Split(',')[0];
            CultureInfo.CurrentUICulture = new CultureInfo(cultureString);

            return commonContext;
        }

        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            return result;
        }
    }
}