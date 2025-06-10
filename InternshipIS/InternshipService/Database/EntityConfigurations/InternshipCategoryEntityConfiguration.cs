using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// InternshipCategory entity configuration
    /// </summary>
    public class InternshipCategoryEntityConfiguration : BaseEntityConfiguration<Guid, InternshipCategory>, IInternshipCategoryEntityConfiguration
    {
    }
}
