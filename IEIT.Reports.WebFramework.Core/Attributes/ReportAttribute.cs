using System;

namespace IEIT.Reports.WebFramework.Core.Attributes
{
    public class ReportAttribute: Attribute
    {
        public string Name { get; set; }
        public ReportAttribute(string Name = null) {
            this.Name = Name;
        }
    }
}
