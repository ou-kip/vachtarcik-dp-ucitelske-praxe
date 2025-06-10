using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// Internship teacher entity configuration interface
    /// </summary>
    public interface IInternshipTeacherEntityConfiguration : IBaseEntityConfiguration<Guid, InternshipTeacher>
    {
    }
}
