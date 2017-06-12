using DocumentFormat.OpenXml;
using $rootnamespace$.Forms.Export.Models.Interfaces;

namespace $rootnamespace$.Forms.Export.Models
{
    public class ExcelCell: IExcelCell
    {
        public string Address { get; set; }
        public string Value { get; set; }
        public bool IsString { get; set; }
        public UInt32Value Style { get; set; }
        public string SheetName { get; set; }
        public ExcelCell(string address, string value, bool isString, UInt32Value style, string sheetName)
        {
            this.Address = address;
            this.Value = value;
            this.IsString = isString;
            this.Style = style;
            this.SheetName = sheetName;
        }
    }
}