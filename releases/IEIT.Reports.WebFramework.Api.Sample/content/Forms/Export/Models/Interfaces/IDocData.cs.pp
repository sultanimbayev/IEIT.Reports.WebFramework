using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;

namespace $rootnamespace$.Forms.Export.Models.Interfaces
{
    public interface IDocData
    {
        Dictionary<string, string> KeyWords { get; set; }
        Dictionary<int, IEnumerable<TableRow>> Tables { get; set; }

    }
}
