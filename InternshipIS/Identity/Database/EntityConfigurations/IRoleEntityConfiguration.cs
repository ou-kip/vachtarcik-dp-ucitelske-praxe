using Core.Database.EntityConfigurations;
using Identity.Database.Entities;

namespace Identity.Database.EntityConfigurations
{
    /// <summary>
    /// The configuration interface for Role entity
    /// </summary>
    public interface IRoleEntityConfiguration : IBaseEntityConfiguration<Guid, Role>
    {
    }
}
