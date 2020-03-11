using IEIT.Reports.WebFramework.Api.Resolvers;
using IEIT.Reports.WebFramework.Core.Attributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestFixture]
    public class ReportResolverTest
    {
        [Test]
        public void GetZipName()
        {
            var actual = FileUtils.GetZipName(typeof(SomeClass));
            Assert.AreEqual("Name1.zip", actual);
        }
        
        [Test]
        public void IsReport()
        {
            var shouldBeFalse = ReportResolver.IsReport("Report3")(typeof(Report1));
            Assert.IsFalse(shouldBeFalse);

            var shouldBeTrue = ReportResolver.IsReport("Report2")(typeof(Report2));
            Assert.IsTrue(shouldBeTrue);
        }
        [Test]
        public void GetFileHandler_ShouldWork()
        {
            var report = ReportResolver.GetFileGeneratorFor("Report2");
            Assert.IsNotNull(report);
            Assert.IsTrue(report is Report2);
        }

        [Test]
        public void ReplaceSuffix()
        {
            Assert.AreEqual("asdf", ReportResolver.RemoveReportSuffix("asdfReport"));
            Assert.AreEqual("asReportdf", ReportResolver.RemoveReportSuffix("asReportdf"));
            Assert.AreEqual("Report1", ReportResolver.RemoveReportSuffix("Report1"));
        }
        /*
         GetAllReportsAndReportsWithHandler
         */
        [Test]
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
