using System;

namespace IEIT.Reports.WebFramework.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RepositoryForAttribute : Attribute
    {
        public string[] FormNames { get; private set; }
        public RepositoryForAttribute(params string[] formNames)
        {
            FormNames = formNames;
        }
    }
}