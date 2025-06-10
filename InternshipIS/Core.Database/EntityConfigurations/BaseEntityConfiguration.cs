using Core.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Database.EntityConfigurations
{
    /// <summary>
    /// The Base entity configuration
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseEntityConfiguration<TKey, TEntity> : IEntityTypeConfiguration<TEntity>, IBaseEntityConfiguration<TKey, TEntity>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Configures the entity properties
        /// </summary>
        /// <param name="builder"></param>
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.CreationDate).IsRequired();
            builder.Property(p => p.CreationAuthor).HasMaxLength(50).IsRequired();
            builder.Property(p => p.UpdatedDate).IsRequired();
            builder.Property(p => p.UpdateAuthor).HasMaxLength(50).IsRequired();
        }
    }
}
