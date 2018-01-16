namespace IEIT.Reports.WebFramework.Core.Interfaces
{
    public interface IHandler: IFileGenerator
    {
        void InitializeRepo(object repository);
    }
}
