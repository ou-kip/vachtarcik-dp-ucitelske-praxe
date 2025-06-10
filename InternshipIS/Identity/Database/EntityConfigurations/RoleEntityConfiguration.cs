using Core.Database.EntityConfigurations;
using Identity.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Database.EntityConfigurations
{
    /// <summary>
    /// The configuration for Role entity
    /// </summary>
    public class RoleEntityConfiguration : BaseEntityConfiguration<Guid, Role>, IRoleEntityConfiguration
    {
        ///<inheritdoc/>
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);
        }
    }
}
