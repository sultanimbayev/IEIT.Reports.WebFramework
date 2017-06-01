using TestProj.BLL.Forms.Export.Models.Interfaces;
using System.Collections.Generic;
namespace TestProj.BLL.Forms.Export.Repositories.Interfaces
{
    public interface ISimpleExcelRepository
    {
        List<string> FileNames { get; set; }

        List<IExcelCell> ExcelCells { get; set; }

        string TemplateId { get; set; }
    }
}
