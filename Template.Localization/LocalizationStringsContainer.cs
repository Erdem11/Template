using Template.Common.Types;

namespace Template.Localization
{
    internal static class LocalizationStringsContainer
    {
        private static readonly LocalizationStrings English = new LocalizationStringsEn();
        private static readonly LocalizationStrings Turkish = new LocalizationStringsTr();

        public static LocalizationStrings Get(Languages language)
        {
            return language switch
            {
                Languages.English => English,
                Languages.Turkish => Turkish,
                _ => English
            };
        }
    }
}