using IEIT.Reports.WebFramework.Core.Attributes;
using IEIT.Reports.WebFramework.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProj.BLL.Forms.Export.Repositories
{
    [RepositoryFor("MyFile")]
    [HasHandler("MyFileHandler")]
    public class MyFileRepo : IRepository
    {
        public string Content { get; set; }

        public void Init(NameValueCollection queryParams)
        {
            var name = queryParams["name"] ?? "всем";
            Content = $"Привет {name}!";
        }
    }
}
