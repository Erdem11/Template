using System;
using System.Linq.Expressions;
using Template.Common.Types;

namespace Template.Localization
{
    public static class Localizer
    {
        public static string Localize(Expression<Func<LocalizationStrings, string>> localizable, params string[] alternatives)
        {
            return Localize(localizable, (Languages)0, alternatives);
        }

        public static string Localize(Expression<Func<LocalizationStrings, string>> localizable, Languages language, params string[] alternatives)
        {
            var result = LocalizationResult(localizable.Name, language, alternatives);

            return result;
        }

        public static string Localize(this string propertyName, Languages language, params string[] alternatives)
        {
            var result = LocalizationResult(propertyName, language, alternatives);

            if (!string.IsNullOrWhiteSpace(result))
                return result;
            result = LocalizationResult(propertyName, Languages.English, alternatives);

            if (!string.IsNullOrWhiteSpace(result))
                return result;

            return propertyName;
        }

        private static string LocalizationResult(string propertyName, Languages language, params string[] alternatives)
        {
            var localizationStrings = LocalizationStringsContainer.Get(language);
            var result = localizationStrings.GetType().GetProperty(propertyName)?.GetValue(localizationStrings)?.ToString();

            if (alternatives != null && result != null)
            {
                result = string.Format(result, alternatives);
            }

            return result;
        }

    }
}