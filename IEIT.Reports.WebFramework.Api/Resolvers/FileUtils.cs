using IEIT.Reports.WebFramework.Core.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;

namespace IEIT.Reports.WebFramework.Api.Resolvers
{
    public static class FileUtils
    {
        private const string DEFAULT_ZIP_NAME = "Выходные данные.zip";

        /// <summary>
        /// Получить название архива для файлов относящиеся к данному репозиторию
        /// </summary>
        /// <param name="repository">Репозитории, для которого требуется это узнать</param>
        /// <returns>Название файла архива или <see cref="string.Empty"/> если не найдено</returns>
        public static string GetZipName(Type repository)
        {
            var name = repository?.GetCustomAttribute<ReturnsZipAttribute>()?.Name ?? DEFAULT_ZIP_NAME;
            return name.EndsWith(".zip") ? name : name + ".zip";
        }






        public static DirectoryInfo CreateDirectoryRecursively(string path)
        {
            string[] pathParts = path.Split('\\');
            DirectoryInfo lastCreatedDir = null;
            for (int i = 0; i < pathParts.Length; i++)
            {
                if (i > 0)
                    pathParts[i] = Path.Combine(pathParts[i - 1] + "\\", pathParts[i]);
                if (!Directory.Exists(pathParts[i]))
                {
                    lastCreatedDir = Directory.CreateDirectory(pathParts[i]);
                }
            }
            return lastCreatedDir;
        }
        public static FileDocument GetZipFile(string fromDir, string zipDir, string zipFileName)
        {
            //Помещаем все файлы в один архив
            var doc = new FileDocument();
            if (!zipFileName.EndsWith(".zip")) { zipFileName += ".zip"; }
            var zipPath = string.Format(@"{0}\{1}", zipDir, zipFileName);
            if (File.Exists(zipPath)) { File.Delete(zipPath); }

            if (!Directory.Exists(zipDir)) { Directory.CreateDirectory(zipDir); }
            ZipFile.CreateFromDirectory(fromDir, zipPath);
            doc.FileName = zipFileName;
            doc.FileContent = File.ReadAllBytes(zipPath);
            return doc;
        }

        /// <summary>
        /// Очистка директории
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static bool DeleteDir(string dirPath)
        {
            try
            {
                Directory.Delete(dirPath, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Проверка на занятость файла другим процессом
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsFileLocked(string filePath)
        {
            try
            {
                using (File.Open(filePath, FileMode.Open))
                {
                }
            }
            catch (IOException e)
            {
                var errorCode = Marshal.GetHRForException(e) & ((1 << 16) - 1);

                return errorCode == 32 || errorCode == 33;
            }

            return false;
        }
        /// <summary>
        /// Получение файла из директорий
        /// </summary>
        /// <param name="resultDir"></param>
        /// <returns></returns>
        public static FileDocument GetOneFile(string resultDir)
        {
            var doc = new FileDocument();

            DirectoryInfo resultDirInfo = new DirectoryInfo(resultDir);
            if (resultDirInfo.GetFiles().Length == 0) { return doc; }
            doc.FileName = resultDirInfo.GetFiles()[0].Name;
            doc.FileContent = File.ReadAllBytes(resultDirInfo.GetFiles()[0].FullName);
            return doc;
        }

        public static HttpResponseMessage GetFileContent(FileDocument doc)
        {
            List<string> files = new List<string>();
            var fileName = doc.FileName;
            var result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(doc.FileContent) };
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = doc.FileName };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.Add("file-type", fileName.Split('.').LastOrDefault());
            result.Content.Headers.Add("file-name", fileName);
            result.Content.Headers.Add("file-name-url-encoded", HttpUtility.UrlPathEncode(fileName));
            return result;
        }
    }
}
