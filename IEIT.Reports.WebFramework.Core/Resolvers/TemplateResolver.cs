using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using System;
using System.Reflection;
using System.Configuration;

namespace IEIT.Reports.WebFramework.Core.Resolvers
{
    /// <summary>
    /// Класс помогающии работать с файлами-шаблонами
    /// </summary>
    public class TemplateResolver
    {
        
        private static string TemplatesDirectory;
        private const string SUPPORTED_FILE_EXTENTIONS = "txt|pdf|rtf|ppt|pptx|xls|xlsx|doc|docx";
        private const string CONFIG_KEY_TEMPLATES_PATH = "WebFramework.TemplatesPath";

        static TemplateResolver()
        {
            // Путь к папке, где хранятся шаблоны
            TemplatesDirectory = ConfigurationManager.AppSettings[CONFIG_KEY_TEMPLATES_PATH];
        }

        /// <summary>
        /// Директория данной сборки
        /// </summary>
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

        /// <summary>
        /// Получить тип файла по его пути
        /// </summary>
        /// <param name="fileId">путь к файлу</param>
        /// <returns>Тип файла</returns>
        private static FileType GetFileType(string fileId)
        {
            var ext = fileId.Split('.').Last();
            if (string.IsNullOrEmpty(ext) || !SUPPORTED_FILE_EXTENTIONS.Split('|').Contains(ext)) { return FileType.UNKNOWN; }
            if (ext.StartsWith("xl")) { return FileType.EXCEL; }
            if (ext.StartsWith("doc")) { return FileType.WORD; }
            return FileType.UNKNOWN;
        } 

        /// <summary>
        /// Перечисление типов файлов
        /// </summary>
        private enum FileType
        {
            EXCEL,
            WORD,
            UNKNOWN
        }

        /// <summary>
        /// Получить путь к файлу шаблона
        /// </summary>
        /// <param name="fileId">Путь к файлу шаблона относлительно директории с шаблонами <see cref="GetTemplatesDir"/></param>
        /// <returns></returns>
        public static string ResolveFilePath(string fileId)
        {
            if (string.IsNullOrEmpty(fileId)) { return null; }
            var baseDir = GetTemplatesDir();

            var targetFileName = fileId;

            if (fileId.Contains("\\"))
            {
                targetFileName = fileId.Substring(fileId.LastIndexOf("\\") + 1);
                var templatePath = fileId.Substring(0, fileId.LastIndexOf("\\"));
                if (!templatePath.StartsWith("\\")) { templatePath = "\\" + templatePath; }
                baseDir = string.Concat(baseDir, templatePath);
            }

            var filesPaths = Directory.GetFiles(baseDir);
            Regex regEx = new Regex($"{targetFileName}(\\.({SUPPORTED_FILE_EXTENTIONS}))?$");

            foreach (var path in filesPaths)
            {
                var fileName = Path.GetFileName(path);
                if (regEx.IsMatch(fileName)) { return path; }
            }

            return null;

        }

        /// <summary>
        /// Получить полный путь к директории с шаблонами
        /// </summary>
        /// <returns>Полный путь к директории с шаблонами</returns>
        public static string GetTemplatesDir() { return Path.GetFullPath($"{AssemblyDirectory}\\{TemplatesDirectory}"); }


    }
}