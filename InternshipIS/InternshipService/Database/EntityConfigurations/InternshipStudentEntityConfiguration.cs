using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// The internship student entity configuration
    /// </summary>
    public class InternshipStudentEntityConfiguration : BaseEntityConfiguration<Guid, InternshipStudent>, IInternshipStudentEntityConfiguration
    {
        ///<inheritdoc/>
        public override void Configure(EntityTypeBuilder<InternshipStudent> builder)
        {
            base.Configure(builder);

            builder
                .HasMany(x => x.Internships)
                .WithMany(y => y.Students)
                .UsingEntity("Internship_Related_Student");

            builder
                .HasIndex(x => x.Name);

            builder
                .HasIndex(x => x.LastName);
        }
    }
}
