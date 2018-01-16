using IEIT.Reports.WebFramework.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEIT.Reports.WebFramework.Core.Attributes
{
    

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DisplayNameAttribute : Attribute
    {
        public DisplayLanguage Lang { get; private set; }
        public string DisplayName { get; private set; }

        public DisplayNameAttribute(DisplayLanguage lang, string name)
        {
            Lang = lang;
            DisplayName = name;
        }
    }
}
