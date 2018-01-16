using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;

namespace TestProj.BLL.Forms.Export.Models.Interfaces
{
    public interface IDocData
    {
        Dictionary<string, string> KeyWords { get; set; }
        Dictionary<int, IEnumerable<TableRow>> Tables { get; set; }

    }
}
