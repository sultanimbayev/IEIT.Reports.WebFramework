using IEIT.Reports.WebFramework.Core.Attributes;
using IEIT.Reports.WebFramework.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [Report]
    [ReturnsZip("CustomName")]
    class Report1 : IReport
    {
        public Report1(NameValueCollection query)
        {

        }
        public void GenerateFiles(string inDir)
        {
            File.WriteAllText(inDir + "/1.txt", "some text");

        }
    }
    [Report]
    class Report2 : IReport
    {
        public Report2(NameValueCollection query)
        {

        }
        public void GenerateFiles(string inDir)
        {
            // create 2 files
            File.WriteAllText(inDir + "/1.txt", "some text");
            File.WriteAllText(inDir + "/2.txt", "some text 2");
        }
    }

    [Report]
    class Report3: IReport
    {
        
        public Report3(NameValueCollection query)
        {
            this.query = query;
        }

        public NameValueCollection query { get; private set; }

        public void GenerateFiles(string inDir)
        {
            File.WriteAllText(inDir + "/1.txt", "some text");
        }
    }
}
