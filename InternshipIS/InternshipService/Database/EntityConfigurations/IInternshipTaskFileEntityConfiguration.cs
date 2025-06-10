using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// Internship task file entity configuration interface
    /// </summary>
    public interface IInternshipTaskFileEntityConfiguration : IBaseEntityConfiguration<Guid, InternshipTaskFile>
    {
    }
}
