using System.Collections.Generic;
using $rootnamespace$.Forms.Export.Models.Interfaces;

namespace $rootnamespace$.Forms.Export.Repositories.Interfaces
{
    public interface IReplacerRepository
    {
        List<IDocBlock> RepeatBlocks { get; set; }

        List<string> FileNames { get; set; }

        IDocData DocData { get; set; }

        string TemplateId { get; set; }

    }
}
