using IEIT.Reports.WebFramework.Core.Enum;
using System;

namespace IEIT.Reports.WebFramework.Api.Resolvers
{
    public static class Utils
    {

        /// <summary>
        /// Перевести название языка в формате ISO 639-1
        /// на объект типа <see cref="DisplayLanguage"/>
        /// </summary>
        /// <param name="langCode">Название языка в формате ISO 639-1</param>
        /// <returns>Объект типа <see cref="DisplayLanguage"/> соответствующее данному названию языка</returns>
        public static DisplayLanguage GetLang(string langCode)
        {
            switch (langCode)
            {
                default:
                    throw new NotImplementedException("Couldn't find language from given short name!");
                case "kk":
                case "kz":
                    return DisplayLanguage.Kazakh;
                case "ru":
                    return DisplayLanguage.Russian;
                case "en":
                    return DisplayLanguage.English;
                case "fr":
                    return DisplayLanguage.French;
                case "it":
                    return DisplayLanguage.Italian;
                case "zh":
                    return DisplayLanguage.Chinese;
                case "ja":
                    return DisplayLanguage.Japan;
                case "ko":
                    return DisplayLanguage.Korean;
                case "de":
                    return DisplayLanguage.German;
                case "ky":
                    return DisplayLanguage.Kyrgyz;
                case "ar":
                    return DisplayLanguage.Arabic;
                case "mn":
                    return DisplayLanguage.Mongolian;
                case "tt":
                    return DisplayLanguage.Tatar;
                case "tr":
                    return DisplayLanguage.Turkish;
                case "uz":
                    return DisplayLanguage.Uzbek;
                case "vi":
                    return DisplayLanguage.Vietnamese;
            }
        }

        /// <summary>
        /// Перевести объект типа <see cref="DisplayLanguage"/>
        /// в название языка в формате ISO 639-1 
        /// </summary>
        /// <param name="lang">Определение языка задающееся объектом типа <see cref="DisplayLanguage"/></param>
        /// <returns>Название языка в формате ISO 639-1</returns>
        public static string ToShortString(DisplayLanguage lang)
        {
            switch (lang)
            {
                default:
                    throw new NotImplementedException("Couldn't find language to get its short name!");
                case DisplayLanguage.Kazakh:
                    return "kk";
                case DisplayLanguage.Russian:
                    return "ru";
                case DisplayLanguage.English:
                    return "en";
                case DisplayLanguage.French:
                    return "fr";
                case DisplayLanguage.Italian:
                    return "it";
                case DisplayLanguage.Chinese:
                    return "zh";
                case DisplayLanguage.Japan:
                    return "ja";
                case DisplayLanguage.Korean:
                    return "ko";
                case DisplayLanguage.German:
                    return "de";
                case DisplayLanguage.Kyrgyz:
                    return "ky";
                case DisplayLanguage.Arabic:
                    return "ar";
                case DisplayLanguage.Mongolian:
                    return "mn";
                case DisplayLanguage.Tatar:
                    return "tt";
                case DisplayLanguage.Turkish:
                    return "tr";
                case DisplayLanguage.Uzbek:
                    return "uz";
                case DisplayLanguage.Vietnamese:
                    return "vi";
            }
        }

    }
}
