using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// The entity configuration for InternshipTaskSolutionFile
    /// </summary>
    public class InternshipTaskSolutionFileEntityConfiguration : BaseEntityConfiguration<Guid, InternshipTaskSolutionFile>, IInternshipTaskSolutionFileEntityConfiguration
    {
    }
}
