using Core.Database.EntityConfigurations;
using Identity.Database.Entities;

namespace Identity.Database.EntityConfigurations
{
    /// <summary>
    /// The configuration interface for User entity
    /// </summary>
    public interface IUserEntityConfiguration : IBaseEntityConfiguration<Guid,User>
    {
    }
}
