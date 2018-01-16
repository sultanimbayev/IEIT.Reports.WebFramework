using System.Linq;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IEIT.Reports.WebFramework.Api.Resolvers;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            

        }
        [TestMethod]
        public void GetAllReports()
        {
            var reports = RepositoryResolver.GetAllReports();
            Assert.IsTrue(reports.SequenceEqual(new List<Type> { new Report1().GetType(), typeof(Report2), typeof(Report3) }));
        }
        [TestMethod]
        public void GetNameOfClass()
        {
            Assert.AreEqual(new Report1().GetType().Name, "Report1");
        }
        [TestMethod]
        public void ReplaceSuffix()
        {
            Assert.AreEqual("asdf", RepositoryResolver.RemoveReportSuffix("asdfReport"));
            Assert.AreEqual("asReportdf", RepositoryResolver.RemoveReportSuffix("asReportdf"));
            Assert.AreEqual("Report1", RepositoryResolver.RemoveReportSuffix("Report1"));
        }
        [TestMethod]
        public void GetReportFor_ShouldThrowWhenNoConstructorWithNameValueCollection()
        {
            try {
                var report = RepositoryResolver.GetReportFor("Report1", new NameValueCollection());
                Assert.Fail();
            }
            catch(EntryPointNotFoundException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        [TestMethod]
        public void GetReportFor_ShouldWork()
        {
            var report = RepositoryResolver.GetReportFor("Report2", new NameValueCollection());
            Assert.IsNotNull(report);
        }
        [TestMethod]
        public void GetReportFor_ShouldReceiveNameValueCollection()
        {
            var query = new NameValueCollection();
            var someKey = "someKey";
            var someValue = "someValue";
            query.Add(someKey, someValue);
            var report = RepositoryResolver.GetReportFor("Report3", query);
            Assert.IsNotNull(report);
            Assert.AreEqual((report as Report3).query[someKey], someValue);

        }
        [TestMethod]
        public void GetFileHandler_ShouldWork()
        {
            RepositoryResolver.InitRepositories();
            var report = RepositoryResolver.GetFileGeneratorFor("Report2", new NameValueCollection());
            Assert.IsNotNull(report);
        }
        [TestMethod]
        public void GetFileHandler_ShouldWorkForHandler()
        {
            RepositoryResolver.InitRepositories();
            var report = RepositoryResolver.GetFileGeneratorFor("Handler", new NameValueCollection());
            Assert.IsNotNull(report);
        }
    }
}
