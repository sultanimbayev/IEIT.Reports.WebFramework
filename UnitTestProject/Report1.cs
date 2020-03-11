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
        public void GenerateFiles(NameValueCollection queryParams, string inDir)
        {
            File.WriteAllText(inDir + "/1.txt", "some text");

        }
    }
    [Report]
    class Report2 : IReport
    {
        public void GenerateFiles(NameValueCollection queryParams, string inDir)
        {
            // create 2 files
            File.WriteAllText(inDir + "/1.txt", "some text");
            File.WriteAllText(inDir + "/2.txt", "some text 2");
        }
    }

    [Report]
    class Report3: IReport
    {
        public void GenerateFiles(NameValueCollection queryParams, string inDir)
        {
            File.WriteAllText(inDir + "/1.txt", "some text");
        }
    }
}
