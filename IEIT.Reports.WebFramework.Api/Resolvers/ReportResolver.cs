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
        public static IFileGenerator GetFileGeneratorFor(string formName)
        {
            var report = GetAllReports().Where(IsReport(formName)).FirstOrDefault();
            if (report == null) return null;
            if (report.GetConstructor(new Type[] { }) == null)
            {
                throw new EntryPointNotFoundException($"Класс '{report.Name}' должен иметь конструктор без аргументов");
            }
            var fileGeneratorInstance = Activator.CreateInstance(report) as IFileGenerator;
            return fileGeneratorInstance;
        }
     
        private static IEnumerable<Type> ReportsCache;
        public static IEnumerable<Type> GetAllReports()
        {
            if(ReportsCache != null) { return ReportsCache; }
            var result = new List<Type>();
            foreach(var assembly in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                AppDomain.CurrentDomain.Load(assembly);
            }
                
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    var reportHandlers = assembly.GetTypes().Where(type => type.GetCustomAttribute<ReportAttribute>() != null);
                    result.AddRange(reportHandlers);
                }
                catch (Exception e)
                {
#if DEBUG
                    throw e;
#endif
                }
            }

            return (ReportsCache = result);
        }
        public static string RemoveReportSuffix(string input) {
            return new Regex("Report$").Replace(input, ""); 
        }
        public static Func<Type, bool> IsReport(string formName)
        {
            return (type) =>
            {
                var reportAttr = type.GetCustomAttribute<ReportAttribute>();
                if (!string.IsNullOrWhiteSpace(reportAttr?.Name))
                {
                    return reportAttr.Name.Trim() == formName;
                }
                return RemoveReportSuffix(type.Name) == formName;
            };
        }
    }
}   