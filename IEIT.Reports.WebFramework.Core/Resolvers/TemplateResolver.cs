using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using System;
using System.Reflection;
using System.Configuration;

namespace IEIT.Reports.WebFramework.Core.Resolvers
{
    public class TemplateResolver
    {
        
        private static string TemplatesDirectory;
        private const string SUPPORTED_FILE_EXTENTIONS = "xls|xlsx|doc|docx";
        private const string CONFIG_KEY_TEMPLATES_PATH = "WebFramework.TemplatesPath";

        static TemplateResolver()
        {
            // Путь к папке, где хранятся шаблоны
            TemplatesDirectory = ConfigurationManager.AppSettings[CONFIG_KEY_TEMPLATES_PATH];
        }

        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }

        private static FileType GetFileType(string fileId)
        {
            var ext = fileId.Split('.').Last();
            if (string.IsNullOrEmpty(ext) || !SUPPORTED_FILE_EXTENTIONS.Split('|').Contains(ext)) { return FileType.UNKNOWN; }
            if (ext.StartsWith("xl")) { return FileType.EXCEL; }
            if (ext.StartsWith("doc")) { return FileType.WORD; }
            return FileType.UNKNOWN;
        } 

        private enum FileType
        {
            EXCEL,
            WORD,
            UNKNOWN
        }

        public static string ResolveFilePath(string fileId)
        {
            var baseDir = GetTemplatesDir();
            if (string.IsNullOrEmpty(fileId))
            {
                return string.Empty;
            }

            var fileExtensions = SUPPORTED_FILE_EXTENTIONS.Split('|').Select(ext => '.' + ext);
            bool idContainsExtension = false;
            foreach (var fileExt in fileExtensions) { if (fileId.EndsWith(fileExt)) { idContainsExtension = true; break; } }

            if (idContainsExtension) { return $"{baseDir}\\{fileId}"; }

            var pathParts = fileId.Split("/\\".ToCharArray());
            var targetFileName = pathParts.Last();
            pathParts = pathParts.Take(pathParts.Count() - 1).ToArray();
            var innerPath = string.Join("\\", pathParts);
            var fileNames = Directory.GetFiles($"{baseDir}\\{innerPath}").Select(path => Path.GetFileName(path));

            Regex regEx = new Regex($"{targetFileName}\\.({SUPPORTED_FILE_EXTENTIONS})");
            foreach (var fileName in fileNames)
            {
                if (regEx.IsMatch(fileName)) { return $"{baseDir}\\{innerPath}\\{fileName}"; }
            }

            return string.Empty;

        }

        private static string GetTemplatesDir() { return Path.GetFullPath($"{AssemblyDirectory}\\{TemplatesDirectory}"); }


    }
}