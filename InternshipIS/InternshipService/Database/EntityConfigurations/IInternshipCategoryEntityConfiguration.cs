using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// InternshipCategory entity configuration interface
    /// </summary>
    public interface IInternshipCategoryEntityConfiguration : IBaseEntityConfiguration<Guid, InternshipCategory>
    {
    }
}
