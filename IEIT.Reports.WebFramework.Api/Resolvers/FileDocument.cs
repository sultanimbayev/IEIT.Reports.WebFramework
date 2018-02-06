namespace IEIT.Reports.WebFramework.Api.Resolvers
{
    /// <summary>
    /// Вспомогательный класс для хранения данных о файле
    /// </summary>
    public class FileDocument
    {
        public int ID { get; set; }
        public int ReferenceID { get; set; }
        //public FileType Type { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }

}