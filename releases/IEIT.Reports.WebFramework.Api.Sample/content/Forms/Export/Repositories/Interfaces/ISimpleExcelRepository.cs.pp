using System.Collections.Generic;
using $rootnamespace$.Forms.Export.Models.Interfaces;

namespace $rootnamespace$.Forms.Export.Repositories.Interfaces
{
    public interface ISimpleExcelRepository
    {
        List<string> FileNames { get; set; }

        List<IExcelCell> ExcelCells { get; set; }

        string TemplateId { get; set; }
    }
}
