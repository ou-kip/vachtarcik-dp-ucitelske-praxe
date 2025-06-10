using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// The entity configuration for InternshipTaskLink
    /// </summary>
    public class InternshipTaskLinkEntityConfiguration : BaseEntityConfiguration<Guid, InternshipTaskLink>, IInternshipTaskLinkEntityConfiguration
    {
        ///<inheritdoc/>
        public override void Configure(EntityTypeBuilder<InternshipTaskLink> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(x => x.Task)
                .WithMany(x => x.Links)
                .HasForeignKey(x => x.TaskId);
        }
    }
}
