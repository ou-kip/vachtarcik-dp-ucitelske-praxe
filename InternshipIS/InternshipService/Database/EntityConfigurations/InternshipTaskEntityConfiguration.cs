using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// The internship task entity configuration
    /// </summary>
    public class InternshipTaskEntityConfiguration : BaseEntityConfiguration<Guid, InternshipTask>, IInternshipTaskEntityConfiguration
    {
        ///<inheritdoc/>
        public override void Configure(EntityTypeBuilder<InternshipTask> builder)
        {
            base.Configure(builder);

            builder.Navigation(x => x.Links).AutoInclude();

            builder
                .HasOne(x => x.Internship)
                .WithMany(y => y.Tasks)
                .HasForeignKey(x => x.InternshipId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(x=>x.Files)
                .WithOne(y=>y.Task)
                .HasForeignKey(y=>y.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
