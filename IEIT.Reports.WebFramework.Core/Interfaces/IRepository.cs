using System.Collections.Specialized;

namespace IEIT.Reports.WebFramework.Core.Interfaces
{
    public interface IRepository
    {
        void Init(NameValueCollection queryParams);
    }
}
