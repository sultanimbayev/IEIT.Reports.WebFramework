using IEIT.Reports.WebFramework.Core.Attributes;
using IEIT.Reports.WebFramework.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IEIT.Reports.WebFramework.Api.Resolvers
{
    public static class LangUtils
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

        private static readonly Dictionary<DisplayLanguage, string> shortNames = new Dictionary<DisplayLanguage, string>
            {
                {DisplayLanguage.Kazakh,"kk"},
                {DisplayLanguage.Russian,"ru"},
                {DisplayLanguage.English,"en"},
                {DisplayLanguage.French,"fr"},
                {DisplayLanguage.Italian,"it"},
                {DisplayLanguage.Chinese,"zh"},
                {DisplayLanguage.Japan,"ja"},
                {DisplayLanguage.Korean,"ko"},
                {DisplayLanguage.German,"de"},
                {DisplayLanguage.Kyrgyz,"ky"},
                {DisplayLanguage.Arabic,"ar"},
                {DisplayLanguage.Mongolian,"mn"},
                {DisplayLanguage.Tatar,"tt"},
                {DisplayLanguage.Turkish,"tr"},
                {DisplayLanguage.Uzbek,"uz"},
                {DisplayLanguage.Vietnamese,"vi"},
            };
        /// <summary>
        /// Получить название отчета на выбранном языке. 
        /// Возвращает null если название не найдено.
        /// </summary>
        /// <param name="formName">Имя отчета в системе выгрузки</param>
        /// <param name="langCode">Код языка в формате ISO 639-1</param>
        /// <returns>Название отчета на выбранном языке, или null если название не найдено</returns>
        public static string GetDisplayName(string formName, string langCode)
        {
            var lang = LangUtils.GetLang(langCode);
            return GetDisplayName(formName, lang);
        }
        /// <summary>
        /// Перевести объект типа <see cref="DisplayLanguage"/>
        /// в название языка в формате ISO 639-1 
        /// </summary>
        /// <param name="lang">Определение языка задающееся объектом типа <see cref="DisplayLanguage"/></param>
        /// <returns>Название языка в формате ISO 639-1</returns>
        public static string ToShortString(DisplayLanguage lang)
        {
            if (!shortNames.ContainsKey(lang))
                throw new NotImplementedException("Couldn't find language to get its short name!");
            return shortNames[lang];
        }
        /// <summary>
        /// Получить название отчета на выбранном языке. 
        /// Возвращает null если название не найдено.
        /// </summary>
        /// <param name="formName">Имя отчета в системе выгрузки</param>
        /// <param name="lang">Язык на котором требуется найти название</param>
        /// <returns>Название отчета на выбранном языке, или null если название не найдено</returns>
        public static string GetDisplayName(string formName, DisplayLanguage lang)
        {
            return ReportResolver.GetAllReports().FirstOrDefault(ReportResolver.IsReport(formName))
                ?.GetCustomAttributes<DisplayNameAttribute>()
                ?.FirstOrDefault(attr => attr.Lang == lang)
                ?.DisplayName;
        }

        /// <summary>
        /// Получить название для всех отчетов на выбранном языке.
        /// Первое значение (значение-ключ) будет именем отчета в системе выгрузки
        /// Второе значение будет null если название на выбранном языке не найдено.
        /// </summary>
        /// <param name="langCode">Код языка в формате ISO 639-1</param>
        /// <returns>Название отчетов на выбранном языке</returns>
        public static Dictionary<string, string> GetDisplayNames(string langCode)
        {
            var lang = GetLang(langCode);
            return GetDisplayNames(lang);
        }

        /// <summary>
        /// Получить название для всех отчетов на выбранном языке.
        /// Первое значение (значение-ключ) будет именем отчета в системе выгрузки
        /// Второе значение будет null если название на выбранном языке не найдено.
        /// </summary>
        /// <param name="lang">Язык на котором требуется найти названия</param>
        /// <returns>Название отчетов на выбранном языке</returns>
        public static Dictionary<string, string> GetDisplayNames(DisplayLanguage lang)
        {
            return ReportResolver.GetAllReports().Select(t => new {
                name = ReportResolver.RemoveReportSuffix(t.Name),
                displayName = t.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault(attr => attr.Lang == lang)?.DisplayName
            })
                .ToDictionary(t => t.name, t => t.displayName);
        }
    }

}
