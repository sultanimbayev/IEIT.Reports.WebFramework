using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using DocumentFormat.OpenXml.Packaging;
using TestProj.BLL.Forms.Export.Models;
using TestProj.BLL.Forms.Export.Repositories.Interfaces;
using TestProj.BLL.Forms.Export.Models.Interfaces;
using IEIT.Reports.WebFramework.Core.Attributes;
using TestProj.BLL.Forms.Export.Handlers;
using IEIT.Reports.WebFramework.Core.Resolvers;
using IEIT.Reports.Export.Helpers.Spreadsheet;
using IEIT.Reports.WebFramework.Core.Interfaces;
using IEIT.Reports.WebFramework.Core.Enum;

namespace TestProj.BLL.Forms.Export.Repositories
{
    [RepositoryFor("HumanExcelInfo")]
    [HasHandler("ExcelFilesHandler")]
    [ReturnsZip("ExcelExamples")]
    [DisplayName(DisplayLanguage.Russian, "Отчет о данных пользователя")]
    public class ExcelRepo : ISimpleExcelRepository, IRepository
    {
        public List<string> FileNames { get; set; }
        public List<IExcelCell> ExcelCells { get; set; }
        public string TemplateId { get; set; } = "excelExample.xlsx";

        public void Init(NameValueCollection queryParams)
        {
            var fileName = $@"ExcelExample {DateTime.Now:dd.MM.yyyy}.xlsx";
            FileNames = new List<string>()
            {
                fileName
            };

            ExcelCells = new List<IExcelCell>();
			
            var templatePath = TemplateResolver.ResolveFilePath(TemplateId);
				
            uint commonStyle = 0;
            uint dateStyle = 0;
            using (var excelDoc = SpreadsheetDocument.Open(templatePath, true))
            {
                commonStyle = excelDoc.GetWorksheet("list").GetCell("A1").StyleIndex;
            }
			
            var name = queryParams.Get("Name");
			var phoneNum = queryParams.Get("phone");
			var region = queryParams.Get("region");
			var workPlace = queryParams.Get("workPlace");
			
			var person = new Person();
			
			person.Name = name == null ? "Иванов Иван" : name;
			person.birthDate = new DateTime(1990, 11, 11);
			person.Phone = phoneNum == null ? "+7(807)7774565" : phoneNum;
			person.region = region == null ? "ЮКО" : region;
			person.workPlace = workPlace == null ? "Google inc." : workPlace;
			
			
			
			
			ExcelCells.Add(new ExcelCell("B3", "Name: ", true, commonStyle, "list"));
			ExcelCells.Add(new ExcelCell("C3", person.Name, true, commonStyle, "list"));
			ExcelCells.Add(new ExcelCell("B4", "BirthDate: ", true, commonStyle, "list"));
			ExcelCells.Add(new ExcelCell("C4", person.birthDate.ToShortDateString(), true, commonStyle, "list"));
			ExcelCells.Add(new ExcelCell("B5", "Phone: ", true, commonStyle, "list"));
			ExcelCells.Add(new ExcelCell("C5", person.Phone, true, commonStyle, "list"));
			ExcelCells.Add(new ExcelCell("B6", "Region: ", true, commonStyle, "list"));
			ExcelCells.Add(new ExcelCell("C6", person.region, true, commonStyle, "list"));
			ExcelCells.Add(new ExcelCell("B7", "WorkPlace: ", true, commonStyle, "list"));
			ExcelCells.Add(new ExcelCell("C7", person.workPlace, true, commonStyle, "list"));
			
			
        }
		
		private class Person{
			public string Name { get; set; }
			public DateTime birthDate {get;set;}
			public string Phone {get;set;}
			public string region {get;set;}
			public string workPlace {get;set;}
			
		}
    }
	

}