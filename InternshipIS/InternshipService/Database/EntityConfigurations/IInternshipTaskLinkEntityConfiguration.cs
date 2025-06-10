using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// The internship task link entity configuration interface
    /// </summary>
    public interface IInternshipTaskLinkEntityConfiguration : IBaseEntityConfiguration<Guid, InternshipTaskLink>
    {
    }
}
