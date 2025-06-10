using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// The internship task file entity configuration
    /// </summary>
    public class InternshipTaskFileEntityConfiguration : BaseEntityConfiguration<Guid, InternshipTaskFile>, IInternshipTaskFileEntityConfiguration
    {
        ///<inheritdoc/>
        public override void Configure(EntityTypeBuilder<InternshipTaskFile> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(x => x.Task)
                .WithMany(y => y.Files)
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
