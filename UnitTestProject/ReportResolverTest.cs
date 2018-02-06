using IEIT.Reports.WebFramework.Api.Resolvers;
using IEIT.Reports.WebFramework.Core.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class ReportResolverTest
    {
        [TestMethod]
        public void GetZipName()
        {
            var actual = FileUtils.GetZipName(typeof(SomeClass));
            Assert.AreEqual("Name1.zip", actual);
        }
        
         [TestMethod]
        public void IsReport()
        {
            var shouldBeFalse = ReportResolver.IsReport("Report3")(typeof(Report1));
            Assert.IsFalse(shouldBeFalse);

            var shouldBeTrue = ReportResolver.IsReport("Report2")(typeof(Report2));
            Assert.IsTrue(shouldBeTrue);
        }
        [TestMethod]
        public void GetFileHandler_ShouldWork()
        {
            var report = ReportResolver.GetFileGeneratorFor("Report2", new NameValueCollection());
            Assert.IsNotNull(report);
            Assert.IsInstanceOfType(report, typeof(Report2));
        }
        [TestMethod]
        public void ReplaceSuffix()
        {
            Assert.AreEqual("asdf", ReportResolver.RemoveReportSuffix("asdfReport"));
            Assert.AreEqual("asReportdf", ReportResolver.RemoveReportSuffix("asReportdf"));
            Assert.AreEqual("Report1", ReportResolver.RemoveReportSuffix("Report1"));
        }
        /*
         GetAllReportsAndReportsWithHandler
         */
        [TestMethod]
        public void GetAllReports()
        {
            var actual = ReportResolver.GetAllReports();

            var expected = new List<Type>()
            {
                typeof(Report1),
                typeof(Report2),
                typeof(Report3)
            };

            CollectionAssert.AreEquivalent(expected, actual.ToList());

        }

         


    }

    [ReturnsZip("Name1")]
    class SomeClass { }
}
