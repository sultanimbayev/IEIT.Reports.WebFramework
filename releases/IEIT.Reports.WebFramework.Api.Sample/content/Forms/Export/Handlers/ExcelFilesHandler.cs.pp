using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using IEIT.Reports.WebFramework.Core.Interfaces;
using IEIT.Reports.WebFramework.Core.Resolvers;
using IEIT.Reports.Export.Helpers.Spreadsheet;
using $rootnamespace$.Forms.Export.Repositories.Interfaces;
using $rootnamespace$.Forms.Export.Helpers;

namespace $rootnamespace$.Forms.Export.Handlers
{
    public class ExcelFilesHandler : IHandler
    {
        public ISimpleExcelRepository Repository {get;set;}

        public void InitializeRepo(object repository)
        {
            if(Repository != null) { return; }
            Repository = repository as ISimpleExcelRepository;
        }

        public void GenerateFiles(string inDir)
        {
            var fileName = Repository.FileNames[0];
            string filePath = $@"{inDir}\{fileName}";
            string filePath2 = $@"{inDir}\2{fileName}";
            var templatePath = TemplateResolver.ResolveFilePath(Repository.TemplateId); //System.Web.Hosting.HostingEnvironment.MapPath($@"\{Template.TemplatePath}\{Template.TemplateName}");
            File.Copy(templatePath, filePath);
            File.Copy(templatePath, filePath2);

            using (var excelDoc = SpreadsheetDocument.Open(filePath, true))
            {
                excelDoc.WorkbookPart.Workbook.CalculationProperties.CalculationOnSave = true;
                excelDoc.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;
                excelDoc.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;

                var worksheetNames = Repository.ExcelCells.Select(c => c.SheetName).Distinct();
                
                foreach (var wsName in worksheetNames)
                {
                    var ws = excelDoc.GetWorksheet(wsName);
                    var data = Repository.ExcelCells.Where(c => c.SheetName == wsName);
                    foreach (var item in data)
                    {
                        ws.Write(item.Value).WithStyle(item.Style).To(item.Address);
                    }
                }

                excelDoc.WorkbookPart.Workbook.Save();
                excelDoc.Close();
            }
        }
    }
}