using TestProj.BLL.Forms.Export.Models.Interfaces;
using System.Collections.Generic;

namespace TestProj.BLL.Forms.Export.Repositories.Interfaces
{
    public interface IReplacerRepository
    {
        List<IDocBlock> RepeatBlocks { get; set; }

        List<string> FileNames { get; set; }

        IDocData DocData { get; set; }

        string TemplateId { get; set; }

    }
}
