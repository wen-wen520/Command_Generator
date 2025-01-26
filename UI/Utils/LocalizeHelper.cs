using System;
using Windows.Globalization;
using Windows.Storage;

namespace Command_Generator
{
    public static class LocalizeHelper
    {
        private const string SelectedLanguageKey = "SelectedLanguage";

        public static void SetLanguage(string language)
        {
            if (!string.IsNullOrEmpty(language))
            {
                ApplicationLanguages.PrimaryLanguageOverride = language;
                SaveLanguageSetting(language);
            }
        }

        public static string GetLanguage()
        {
            return ApplicationLanguages.PrimaryLanguageOverride;
        }

        private static void SaveLanguageSetting(string language)
        {
            ApplicationData.Current.LocalSettings.Values[SelectedLanguageKey] = language;
        }

        public static string LoadLanguageSetting()
        {
            return ApplicationData.Current.LocalSettings.Values[SelectedLanguageKey]?.ToString();
        }


        public static void InitializeAppLanguage()
        {
            string savedLanguage = LoadLanguageSetting();
            
            if (string.IsNullOrEmpty(savedLanguage))
            {

                SetLanguage(ApplicationLanguages.Languages[0]);

            }
            else
            {
                SetLanguage(savedLanguage);
            }

        }
    }
}