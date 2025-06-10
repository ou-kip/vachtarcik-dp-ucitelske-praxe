using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// The internship teacher entity configuration
    /// </summary>
    public class InternshipTeacherEntityConfiguration : BaseEntityConfiguration<Guid, InternshipTeacher>, IInternshipTeacherEntityConfiguration
    {
        ///<inheritdoc/>
        public override void Configure(EntityTypeBuilder<InternshipTeacher> builder)
        {
            base.Configure(builder);

            builder
               .HasMany(x => x.Internships)
               .WithMany(y => y.Teachers)
               .UsingEntity("Internship_Related_Teacher");

            builder
                .HasIndex(x => x.Name);

            builder
                .HasIndex(x => x.LastName);
        }
    }
}
