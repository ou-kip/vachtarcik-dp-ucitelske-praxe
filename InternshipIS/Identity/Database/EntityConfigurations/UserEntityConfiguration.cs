using Core.Database.EntityConfigurations;
using Identity.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Database.EntityConfigurations
{
    /// <summary>
    /// The configuration for user entity
    /// </summary>
    public class UserEntityConfiguration : BaseEntityConfiguration<Guid, User>, IUserEntityConfiguration
    {
        ///<inheritdoc/>
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
        }
    }
}
