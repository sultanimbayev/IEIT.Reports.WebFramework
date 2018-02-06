using IEIT.Reports.WebFramework.Api.Resolvers;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace TestProj.Controllers
{
    public class ReportExportController : ApiController
    {
        private const string API_ROUTE_BASE = "api/Files/DownloadForm/";

        /// <summary>
        /// Для скачивания форм
        /// </summary>
        /// <param name="formName">Название формы</param>
        /// <returns>HTTP ответ с файлом указанной формы или архивом содержащий запрошенные отчеты</returns>
        [HttpGet]
        [Route(API_ROUTE_BASE + "{formName}")]
        public HttpResponseMessage DownloadForm(string formName)
        {
            var tempDir = System.Web.Hosting.HostingEnvironment.MapPath("\\App_Data\\Temp");
            var queryParams = HttpContext.Current.Request.QueryString;
            return Utils.GetResult(formName, queryParams, tempDir);
        }
    }
}