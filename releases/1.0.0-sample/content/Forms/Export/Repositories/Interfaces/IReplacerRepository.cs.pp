using $rootnamespace$.Forms.Export.Models.Interfaces;
using System.Collections.Generic;

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
