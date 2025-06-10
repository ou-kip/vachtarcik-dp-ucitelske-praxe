using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// The entity configuration for InternshipTaskSolution
    /// </summary>
    public class InternshipTaskSolutionEntityConfiguration : BaseEntityConfiguration<Guid, InternshipTaskSolution>, IInternshipTaskSolutionEntityConfiguration
    {
        ///<inheritdoc/>
        public override void Configure(EntityTypeBuilder<InternshipTaskSolution> builder)
        {
            base.Configure(builder);

            builder
                .HasMany(x => x.Files)
                .WithOne(y => y.Solution)
                .HasForeignKey(y => y.SolutionId)
                .IsRequired(false);

            builder
                .HasOne(x => x.Task)
                .WithOne(y => y.Solution).HasForeignKey<InternshipTaskSolution>(x => x.TaskId)
                .IsRequired();
        }
    }
}
