using System;
using IEIT.Reports.WebFramework.Core.Attributes;
using IEIT.Reports.WebFramework.Core.Interfaces;
using System.Collections.Specialized;

namespace UnitTestProject
{
    
    class Handler : IHandler
    {
        
        public void GenerateFiles(string inDir)
        {
            throw new NotImplementedException();
        }

        public void InitializeRepo(object repository)
        {
            //throw new NotImplementedException();
        }
    }
    //[HasHandler("Handler")]
    //[RepositoryFor("Handler")]
    class Repository
    {
        public Repository(NameValueCollection c)
        {

        }
    }
}
