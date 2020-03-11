using System.IO;
using DocumentFormat.OpenXml.Packaging;
using IEIT.Reports.WebFramework.Core.Interfaces;
using IEIT.TemplateResolver;
using TestProj.BLL.Forms.Export.Helpers;
using TestProj.BLL.Forms.Export.Repositories.Interfaces;

namespace TestProj.BLL.Forms.Export.Handlers
{
    public class WordFileHandler//: IHandler
    {
        public IReplacerRepository Repository;

        public void GenerateFiles(string inDir)
        {
            var fileName = Repository.FileNames[0];
            string filePath = $@"{inDir}\{fileName}";
            var templatePath = TemplateResolver.ResolveFilePath(Repository.TemplateId);

            File.Copy(templatePath, filePath);
            using (var wordprocessingDocument = WordprocessingDocument.Open(filePath, true))
            {
                wordprocessingDocument.ReplaceBlocks(Repository.RepeatBlocks);
                wordprocessingDocument.ReplaceKeywords(Repository.DocData.KeyWords);
                wordprocessingDocument.FillTables(Repository.DocData.Tables);
                wordprocessingDocument.MainDocumentPart.Document.Save();
                wordprocessingDocument.Close();
            }
        }

        public void InitializeRepo(object repository)
        {
            if (Repository != null) { return; }
            Repository = repository as IReplacerRepository;
        }
        
    }
}