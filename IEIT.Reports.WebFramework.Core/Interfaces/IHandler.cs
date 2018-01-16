namespace IEIT.Reports.WebFramework.Core.Interfaces
{
    public interface IHandler
    {
        void InitializeRepo(object repository);
        void GenerateFiles(string inDir);
    }
}
