using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Collections.Specialized;
using IEIT.Reports.WebFramework.Core.Interfaces;
using System.Reflection;
using IEIT.Reports.WebFramework.Core.Attributes;

namespace IEIT.Reports.WebFramework.Api.Resolvers
{
    public static class Utils
    {
        
        public static HttpResponseMessage GetResult(string formName, NameValueCollection queryParams, string tempDir)
        {
            var guid = Guid.NewGuid();
            var resultDirPath = $@"{tempDir}\{DateTime.Now:dd.MM.yyyy}_Files_{guid}";
            var zipDirPath = $@"{tempDir}\{DateTime.Now:dd.MM.yyyy}_Archives_{guid}";
            return GetResult(resultDirPath, zipDirPath, formName, queryParams);
        }

        private static HttpResponseMessage GetResult(string resultDirPath, string zipDirPath, string formName, NameValueCollection queryParams)
        {
            var resultDir = FileUtils.CreateDirectoryRecursively(resultDirPath);
            var result = GetResult(resultDir, zipDirPath, formName, queryParams);
            FileUtils.DeleteDir(resultDirPath);
            return result;
        }
        public static HttpResponseMessage GetResult(DirectoryInfo resultDir, string zipDirPath, string formName, NameValueCollection queryParams)
        {
            IFileGenerator fileGenerator = ReportResolver.GetFileGeneratorFor(formName, queryParams);
            var result = new HttpResponseMessage();
            if (fileGenerator == null)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                return result;
            }
            fileGenerator.GenerateFiles(resultDir.FullName);
            result = FileUtils.GetFileContent(GetDocument(fileGenerator.GetType(), resultDir, zipDirPath));
            return result;
        }
        public static FileDocument GetDocument(Type reportType, DirectoryInfo resultDir, string zipDirPath)
        {
            var zipName = FileUtils.GetZipName(reportType);
            var hasAttribute = reportType.GetCustomAttribute<ReturnsZipAttribute>() != null;
            var hasAlotOfFiles = resultDir.GetFiles().Length > 1;
            var willReturnZip = hasAttribute || hasAlotOfFiles;
            return willReturnZip ?
                GetZipResult(zipDirPath, zipName, resultDir) :
                FileUtils.GetOneFile(resultDir.FullName);
        }
        public static FileDocument GetZipResult(string zipDirPath, string zipName, DirectoryInfo resultDir)
        {
            var zipDir = FileUtils.CreateDirectoryRecursively(zipDirPath);
            var doc = FileUtils.GetZipFile(resultDir.FullName, zipDirPath, zipName);
            FileUtils.DeleteDir(zipDirPath);
            return doc;
        }
    }

}
