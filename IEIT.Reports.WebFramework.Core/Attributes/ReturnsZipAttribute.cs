using System;

namespace IEIT.Reports.WebFramework.Core.Attributes
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ReturnsZipAttribute : Attribute
    {
        public string Name;
        public ReturnsZipAttribute(string archiveName) { Name = archiveName; }
    }
}