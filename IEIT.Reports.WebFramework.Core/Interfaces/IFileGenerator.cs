using System.Collections.Specialized;

namespace IEIT.Reports.WebFramework.Core.Interfaces
{
    public interface IFileGenerator
    {
        void GenerateFiles(NameValueCollection queryParams, string inDir);
    }
}
