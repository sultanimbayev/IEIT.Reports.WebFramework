using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;
using TestProj.BLL.Forms.Export.Models.Interfaces;

namespace TestProj.BLL.Forms.Export.Models
{
    public class DocData : IDocData
    {

        public DocData()
        {
            KeyWords = new Dictionary<string, string>();
            Tables = new Dictionary<int, IEnumerable<TableRow>>();
        }

        public DocData(Dictionary<string, string> keyWords, Dictionary<int, IEnumerable<TableRow>> tables)
        {
            this.KeyWords = keyWords;

            this.Tables = tables;
        }

        public Dictionary<string, string> KeyWords { get; set; }
        public Dictionary<int, IEnumerable<TableRow>> Tables { get; set; }
    }
}