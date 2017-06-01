using IEIT.Reports.Export.WebFramework.Resolvers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;

namespace $rootnamespace$.Forms
{
    public class ExportController : ApiController
    {
        private const string DEFAULT_ZIP_NAME = "Выходные данные.zip";

        /// <summary>
        /// Для скачивания форм
        /// </summary>
        /// <param name="formName">Название формы</param>
        /// <returns>HTTP ответ с файлом указанной формы или архивом содержащий запрошенные отчеты</returns>
        [HttpGet]
        [Route("api/Files/DownloadForm/{formName}")]
        public HttpResponseMessage DownloadForm(string formName)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var queryParams = HttpContext.Current.Request.QueryString;

            //var template = RepositoryResolver.Templates.ContainsKey(formName) ? RepositoryResolver.Templates[formName] : null;

            //queryParams.Add("templatePath", template.TemplatePath + template.TemplateName);
            var handler = RepositoryResolver.GetHandlerFor(formName, queryParams);

            if (handler == null)
                return result;

            var tempDir = System.Web.Hosting.HostingEnvironment.MapPath("\\App_Data\\Temp");
            var guid = Guid.NewGuid();
            var resultDirPath = $@"{tempDir}\{DateTime.Now:dd.MM.yyyy}_Files_{guid}";
            var zipDirPath = $@"{tempDir}\{DateTime.Now:dd.MM.yyyy}_Archives_{guid}";

            var resultDir = CreateDirectoryRecursively(resultDirPath);

            if (resultDir == null)
            {
                return result;
            }

            handler.GenerateFiles(resultDirPath);

            if (WillRetunZip(resultDirPath, RepositoryResolver.Repositories[formName]))
            {
                var zipDir = CreateDirectoryRecursively(zipDirPath);
                var zipName = RepositoryResolver.GetZipName(RepositoryResolver.Repositories[formName]);
                var doc = GetZipFile(resultDirPath, zipDirPath, zipName);
                result = GetFileContent(doc);
                DeleteDir(zipDirPath);
            }
            else
            {
                var doc = GetOneFile(resultDirPath);
                result = GetFileContent(doc);
            }

            DeleteDir(resultDirPath);

            return result;
        }

        [NonAction]
        DirectoryInfo CreateDirectoryRecursively(string path)
        {
            string[] pathParts = path.Split('\\');
            DirectoryInfo lastCreatedDir = null;

            for (int i = 0; i < pathParts.Length; i++)
            {
                if (i > 0)
                    pathParts[i] = Path.Combine(pathParts[i - 1] + "\\", pathParts[i]);

                if (!Directory.Exists(pathParts[i]))
                {
                    try
                    {
                        lastCreatedDir = Directory.CreateDirectory(pathParts[i]);
                    }
                    catch (Exception e)
                    {
#if DEBUG
                        Console.WriteLine("Error: {0};\r\nTrace: {1}", e.ToString(), e.StackTrace);
#endif
                        return null;
                    }
                }
            }

            return lastCreatedDir;

        }

        [NonAction]
        private FileDocument GetZipFile(string fromDir, string zipDir, string zipFileName)
        {
            //Помещаем все файлы в один архив
            var doc = new FileDocument();
            if (string.IsNullOrEmpty(zipFileName)) { zipFileName = DEFAULT_ZIP_NAME; }
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
        /// Получение файла из директорий
        /// </summary>
        /// <param name="resultDir"></param>
        /// <returns></returns>
        [NonAction]
        private FileDocument GetOneFile(string resultDir)
        {
            var doc = new FileDocument();

            DirectoryInfo resultDirInfo = new DirectoryInfo(resultDir);
            if (resultDirInfo.GetFiles().Length == 0) { return doc; }
            doc.FileName = resultDirInfo.GetFiles()[0].Name;
            doc.FileContent = File.ReadAllBytes(resultDirInfo.GetFiles()[0].FullName);
            return doc;
        }

        [NonAction]
        bool WillRetunZip(string resultDir, Type repositoryType = null)
        {
            DirectoryInfo resultDirInfo = new DirectoryInfo(resultDir);
            return resultDirInfo.GetFiles().Length > 1 || RepositoryResolver.DoesReturnZip(repositoryType);
        }



        /// <summary>
        /// Получение в респонс файлов из директорий и вызов метода очищения
        /// </summary>
        /// <param name="resultDir"></param>
        /// <param name="zipDir"></param>
        /// <returns></returns>
        [NonAction]
        HttpResponseMessage GetFileContent(FileDocument doc)
        {
            List<string> files = new List<string>();
            var fileName = doc.FileName;
            var result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(doc.FileContent) };
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = doc.FileName };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.Add("file-type", fileName.Split('.').LastOrDefault());
            result.Content.Headers.Add("file-name", fileName);
            return result;
        }

        /// <summary>
        /// Очистка директории
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        [NonAction]
        private bool DeleteDir(string dirPath)
        {
            var resultDirInfo = new DirectoryInfo(dirPath);
            var deleted = false;
            foreach (FileInfo file in resultDirInfo.GetFiles())
            {
                if (!IsFileLocked(file.FullName))
                {
                    file.Delete();
                }
            }
            foreach (DirectoryInfo dir in resultDirInfo.GetDirectories())
            {
                dir.Delete(true);
            }

            if (resultDirInfo.EnumerateDirectories().Count() == 0 && resultDirInfo.EnumerateFiles().Count() == 0)
            {
                Directory.Delete(dirPath, true);
                deleted = true;
            }

            return deleted;

        }

        /// <summary>
        /// Проверка на занятость файла другим процессом
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [NonAction]
        private bool IsFileLocked(string filePath)
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

    }

    /// <summary>
    /// Вспомогательный класс для хранения данных о файле
    /// </summary>
    public class FileDocument
    {
        public int ID { get; set; }
        public int ReferenceID { get; set; }
        //public FileType Type { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }

}
