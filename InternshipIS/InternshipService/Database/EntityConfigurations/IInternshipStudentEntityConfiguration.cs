using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// Internship student entity configuration interface
    /// </summary>
    public interface IInternshipStudentEntityConfiguration : IBaseEntityConfiguration<Guid, InternshipStudent>
    {
    }
}
