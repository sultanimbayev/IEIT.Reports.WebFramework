using IEIT.Reports.WebFramework.Api.Resolvers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestFixture]
    public class UtilsTest
    {
        string GetDebugFolder => Path.GetDirectoryName(Assembly.GetAssembly(typeof(UtilsTest)).Location);
        string GetTempFolder => GetDebugFolder + "/temp" + DateTime.Now.Ticks;
        [Test]
        public void ReturnNotFoundWhenFileGeneratorIsNull()
        {
            var temp = GetTempFolder;
            var res = Utils.GetResult("Not existed report", new NameValueCollection(), temp);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, res.StatusCode);
        }
        [Test]
        public void ReturnZipWhen2FilesOrMore()
        {
            var tempDir = GetTempFolder;
            var res = Utils.GetResult("Report2", new NameValueCollection(), tempDir);
            Assert.AreEqual("Выходные данные.zip", res.Content.Headers.ContentDisposition.FileName);
        }
        [Test]
        public void ReturnZipWhenZipAttributeEvenIfThereIsOneFile()
        {
            var tempDir = GetTempFolder;
            var res = Utils.GetResult("Report1", new NameValueCollection(), tempDir);
            Assert.AreEqual("CustomName.zip", res.Content.Headers.ContentDisposition.FileName);
        }
        [Test]
        public void ReturnFileWhen1File()
        {
            var tempDir = GetTempFolder;
            var res = Utils.GetResult("Report3", new NameValueCollection(), tempDir);
            Assert.AreEqual("1.txt", res.Content.Headers.ContentDisposition.FileName);
        }
        [Test]
        public void ShouldDeleteFolder()
        {
            var tempDir = GetTempFolder;
            var res = Utils.GetResult("Report3", new NameValueCollection(), tempDir);
            Assert.AreEqual("1.txt", res.Content.Headers.ContentDisposition.FileName);
            Assert.AreEqual(0, Directory.GetFiles(tempDir).Length);
            Assert.AreEqual(0, Directory.GetDirectories(tempDir).Length);
        }
    }
}
