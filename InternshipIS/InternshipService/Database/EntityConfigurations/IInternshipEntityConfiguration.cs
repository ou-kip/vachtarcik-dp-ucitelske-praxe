using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// Internship entity configuration interface
    /// </summary>
    public interface IInternshipEntityConfiguration : IBaseEntityConfiguration<Guid, Internship>
    {
    }
}
