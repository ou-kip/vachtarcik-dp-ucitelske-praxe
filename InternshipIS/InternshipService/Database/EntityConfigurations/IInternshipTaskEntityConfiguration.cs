using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// Internship task entity configuration interface
    /// </summary>
    public interface IInternshipTaskEntityConfiguration : IBaseEntityConfiguration<Guid, InternshipTask>
    {
    }
}
