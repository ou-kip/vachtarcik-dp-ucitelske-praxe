using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// The internship link entity configuration
    /// </summary>
    public class InternshipLinkEntityConfiguration : BaseEntityConfiguration<Guid, InternshipLink>, IInternshipLinkEntityConfiguration
    {
        ///<inheritdoc/>
        public override void Configure(EntityTypeBuilder<InternshipLink> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(x => x.Internship)
                .WithMany(x => x.Links)
                .HasForeignKey(x => x.InternshipId);
        }
    }
}
