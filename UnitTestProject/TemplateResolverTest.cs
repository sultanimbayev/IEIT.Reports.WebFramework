using IEIT.Reports.WebFramework.Core.Resolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class TemplateResolverTest
    {
        [TestMethod]
        public void GetTemplatesDir()
        {
            var actual = TemplateResolver.GetTemplatesDir();
            var currentDir = Directory.GetCurrentDirectory();
            var path = ConfigurationManager.AppSettings["WebFramework.TemplatesPath"];
            var expectedTemplatePath = Path.GetFullPath(Path.Combine(currentDir, path));
            Assert.AreEqual(expectedTemplatePath, actual);
        }

        [TestMethod]
        public void GetFilePaths()
        {
            var actual = TemplateResolver.ResolveFilePath("text\\2");
            var expected = TemplateResolver.GetTemplatesDir() + "\\text\\2.txt";
            Assert.AreEqual(expected, actual);

            actual = TemplateResolver.ResolveFilePath("text\\2.txt");
            Assert.AreEqual(expected, actual);

            actual = TemplateResolver.ResolveFilePath("1");
            expected = TemplateResolver.GetTemplatesDir() + "\\1.txt";
            Assert.AreEqual(expected, actual);

            actual = TemplateResolver.ResolveFilePath("1.txt");
            Assert.AreEqual(expected, actual);
        }
    } 
}
