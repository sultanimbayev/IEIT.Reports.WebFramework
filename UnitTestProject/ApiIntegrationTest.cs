using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using NUnit.Framework;

namespace UnitTestProject
{
    [TestFixture]
    public class ApiIntegrationTest
    {
        string GetTempFolder => GetDebugFolder + "/temp" + DateTime.Now.Ticks;
        string GetDebugFolder => Path.GetDirectoryName(Assembly.GetAssembly(typeof(UtilsTest)).Location);

        [Test]
        public async Task ShouldOpenZipReport()
        {
            HttpClient client = new HttpClient() {
                BaseAddress = new Uri("http://localhost:38276")
            };
            var response = await client.GetAsync("/api/Files/DownloadForm/HumanExcelInfo?date=20150101");
            var bytes = await response.Content.ReadAsByteArrayAsync();
            var temp = GetTempFolder;
            Directory.CreateDirectory(temp);
            var filename = temp + '/' + response.Content.Headers.ContentDisposition.FileName;
            File.WriteAllBytes(filename, bytes);
            System.Diagnostics.Process.Start(filename);
        }
        [Test]
        public async Task ShouldOpenDocReport()
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:38276")
            };
            var response = await client.GetAsync("/api/Files/DownloadForm/HumanInfo");
            var bytes = await response.Content.ReadAsByteArrayAsync();
            var temp = GetTempFolder;
            Directory.CreateDirectory(temp);
            // HACK!!! Remove ".
            var filename = temp + '/' + response.Content.Headers.ContentDisposition.FileName.Replace("\"","");
            File.WriteAllBytes(filename, bytes);
            System.Diagnostics.Process.Start(filename);
        }
    }
}
