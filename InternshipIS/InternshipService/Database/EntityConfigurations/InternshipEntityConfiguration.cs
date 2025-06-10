using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// The internship entity configuration
    /// </summary>
    public class InternshipEntityConfiguration : BaseEntityConfiguration<Guid, Internship>, IInternshipEntityConfiguration
    {
        ///<inheritdoc/>
        public override void Configure(EntityTypeBuilder<Internship> builder)
        {
            base.Configure(builder);

            builder.Navigation(x => x.Students).AutoInclude();
            builder.Navigation(x => x.Teachers).AutoInclude();
            builder.Navigation(x => x.CompanyRelatives).AutoInclude();
            builder.Navigation(x => x.Links).AutoInclude();
            builder.Navigation(x => x.InternshipCategory).AutoInclude();

            builder
                .Property(x => x.Name)
                .HasMaxLength(128)
                .IsRequired();

            builder
                .Property(x => x.SelectedOn)
                .IsRequired(false);

            builder
                .Property(x => x.PublishedOn)
                .IsRequired(false);

            builder
                .Property(x => x.CanceledOn)
                .IsRequired(false);

            builder
                .Property(x => x.FinishedOn)
                .IsRequired(false);

            builder
                .Property(x => x.Description)
                .IsRequired(true);

            builder
                .HasMany(x => x.Students)
                .WithMany(y => y.Internships)
                .UsingEntity("Internship_Related_Student");

            builder
                .HasMany(x => x.Teachers)
                .WithMany(y => y.Internships)
                .UsingEntity("Internship_Related_Teacher");
            builder
                .HasMany(x => x.CompanyRelatives)
                .WithMany(y => y.Internships)
                .UsingEntity("Internship_Related_Person");

            builder
                .HasMany(x => x.Tasks)
                .WithOne(y => y.Internship)
                .HasForeignKey(y => y.InternshipId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.InternshipCategory)
                .WithMany(y => y.Internships)
                .HasForeignKey(y => y.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasIndex(x => x.Name);
        }
    }
}
