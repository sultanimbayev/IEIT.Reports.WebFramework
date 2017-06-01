using DocumentFormat.OpenXml;

namespace $rootnamespace$.Forms.Export.Models.Interfaces
{
    public interface IExcelCell
    {
        string Address { get; set; }
        string Value { get; set; }
        bool IsString { get; set; }
        UInt32Value Style { get; set; }
        string SheetName { get; set; }
    }
}
