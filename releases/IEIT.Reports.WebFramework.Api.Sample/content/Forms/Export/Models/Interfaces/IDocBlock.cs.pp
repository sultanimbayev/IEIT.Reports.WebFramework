using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $rootnamespace$.Forms.Export.Models.Interfaces
{
    public interface IDocBlock
    {
        string StartKeyWord { get; set; }
        string EndKeyWord { get; set; }

        List<IDocData> DocDataList { get; set; }
        
    }
}
