using IEIT.Reports.WebFramework.Core.Attributes;
using IEIT.Reports.WebFramework.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [Report]
    class Report1 : IReport
    {
        public void GenerateFiles(string inDir)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
