using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using TestProj.BLL.Forms.Export.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using TestProj.BLL.Forms.Export.Repositories.Interfaces;
using TestProj.BLL.Forms.Export.Models.Interfaces;
using TestProj.BLL.Forms.Export.Handlers;
using IEIT.Reports.WebFramework.Core.Attributes;
using IEIT.Reports.WebFramework.Core.Interfaces;

namespace TestProj.BLL.Forms.Export.Repositories
{
    //[RepositoryFor("HumanInfo")]
    //[HasHandler(typeof(WordFileHandler))]
    [Report]
    public class HumanInfoReport : IReport, IReplacerRepository
    {
        public List<string> FileNames { get; set; }

        public List<IDocBlock> RepeatBlocks { get;set; }

        public IDocData DocData { get; set;}

        public string TemplateId { get; set; } = "wordExample";


        public void Init(NameValueCollection queryParams)
        {
            DocData = new DocData();

            var person = new Person();

            var name = queryParams.Get("Name");
            var phoneNum = queryParams.Get("phone");
            var region = queryParams.Get("region");
            var workPlace = queryParams.Get("workPlace");

            person.Name = name == null ? "Иванов Иван" : name;
            person.birthDate = new DateTime(1990, 11, 11);
            person.Phone = phoneNum == null ? "+78077774565" : phoneNum;
            person.region = region == null ? "ЮКО" : region;
            person.workPlace = workPlace == null ? "Google inc." : workPlace;

            DocData.Tables = new Dictionary<int, IEnumerable<TableRow>>();

            DateTime now = DateTime.Now;
            FileNames = new List<string>()
            {
                $"Human {name} details {now.ToString("yyyy MM dd")}.docx"
            };

            DocData.KeyWords = new Dictionary<string, string>()
            {
                {"RRname", person.Name}, // ФИО
				{"RRbirthdate", person.birthDate.ToShortDateString()}, // Дата рождения
				{"RRphone", person.Phone}, // Телефон
				{"RRregion", person.region}, // Область
				{"RRworkPlace", person.workPlace} // Место работы
			};
        }
			
		private class Person{
			public string Name { get; set; }
			public DateTime birthDate {get;set;}
			public string Phone {get;set;}
			public string region {get;set;}
			public string workPlace {get;set;}
		}

        public void GenerateFiles(NameValueCollection queryParams, string inDir)
        {
            Init(queryParams);
            var h = new WordFileHandler();
            h.InitializeRepo(this);
            h.GenerateFiles(inDir);
        }
    }
}