using IEIT.Reports.WebFramework.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProj.BLL.Forms.Export.Repositories;

namespace TestProj.BLL.Forms.Export.Handlers
{
    public class MyFileHandler : IHandler
    {
        MyFileRepo repo;
        public void InitializeRepo(object repository)
        {
            repo = (MyFileRepo)repository;
        }
    
        public void GenerateFiles(string inDir)
        {
            var filePath = Path.Combine(inDir, "file.txt");
            File.WriteAllText(filePath, repo.Content);
        }

    }
}
