using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using DocumentFormat.OpenXml.Wordprocessing;
using IEIT.Reports.WebFramework.Core.Attributes;
using $rootnamespace$.Forms.Export.Models;
using $rootnamespace$.Forms.Export.Repositories.Interfaces;
using $rootnamespace$.Forms.Export.Models.Interfaces;
using $rootnamespace$.Forms.Export.Handlers;

namespace $rootnamespace$.Forms.Export.Repositories
{
    [RepositoryFor("HumanInfo")]
    [HasHandler(typeof(WordFileHandler))]
    public class HumanDetailsRepo: IReplacerRepository
    {
        public List<string> FileNames { get; set; }

        public List<IDocBlock> RepeatBlocks { get;set; }

        public IDocData DocData { get; set;}

        public string TemplateId { get; set; } = "wordExample";


        public HumanDetailsRepo(NameValueCollection queryParams)
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
		

    }
}