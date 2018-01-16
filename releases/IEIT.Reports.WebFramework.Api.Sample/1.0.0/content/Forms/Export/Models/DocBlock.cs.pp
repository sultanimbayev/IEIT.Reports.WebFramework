using System.Collections.Generic;
using $rootnamespace$.Forms.Export.Models.Interfaces;

namespace $rootnamespace$.Forms.Export.Models
{
    public class DocBlock: IDocBlock
    {
        public string StartKeyWord { get; set; }
        public string EndKeyWord { get; set; }
        public List<IDocData> DocDataList { get; set; }

        public DocBlock(string startKeyWord, string endKeyWord)
        {
            this.StartKeyWord = startKeyWord;
            this.EndKeyWord = endKeyWord;
            DocDataList = new List<IDocData>();
        }

        public DocBlock(string startKeyWord, string endKeyWord, List<IDocData> docDataList) : this(startKeyWord, endKeyWord)
        {
            this.DocDataList = docDataList;
        }

    }
}