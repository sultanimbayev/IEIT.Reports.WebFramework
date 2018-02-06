using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using IEIT.Reports.WebFramework.Core.Interfaces;
using IEIT.Reports.WebFramework.Core.Attributes;
using System.Text.RegularExpressions;
using IEIT.Reports.WebFramework.Core.Enum;
using System.Reflection;

namespace IEIT.Reports.WebFramework.Api.Resolvers
{
    /// <summary>
    /// Класс для работы с репозиториями выгружаемых форм
    /// </summary>
    public static class ReportResolver
    {
        public static IFileGenerator GetFileGeneratorFor(string formName, NameValueCollection query)
        {
            var report = GetAllReports().Where(IsReport(formName)).FirstOrDefault();
            if (report == null) return null;
            if (report.GetConstructor(new Type[] { typeof(NameValueCollection) }) == null)
            {
                throw new EntryPointNotFoundException($"Класс '{report.Name}' должен иметь конструктор принимающий NameValueCollection");
            }
            var fileGeneratorInstance = Activator.CreateInstance(report, new object[] { query }) as IFileGenerator;
            return fileGeneratorInstance;
        }
     
        private static IEnumerable<Type> ReportsCache;
        public static IEnumerable<Type> GetAllReports()
        {
            return ReportsCache ?? (
                ReportsCache = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(type =>  type.GetCustomAttribute<ReportAttribute>() != null)
                );
        }
        public static string RemoveReportSuffix(string input) {
            return new Regex("Report$").Replace(input, ""); 
        }
        public static Func<Type, bool> IsReport(string formName)
        {
            return (type) => RemoveReportSuffix(type.Name) == formName;
        }
    }
}   