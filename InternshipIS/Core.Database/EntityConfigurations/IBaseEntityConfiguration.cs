using Core.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Database.EntityConfigurations
{
    /// <summary>
	/// The Base Entity Configuration interface
	/// </summary>
	public interface IBaseEntityConfiguration<TKey, TEntity>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Configures the entity/table
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<TEntity> builder);
    }
}
