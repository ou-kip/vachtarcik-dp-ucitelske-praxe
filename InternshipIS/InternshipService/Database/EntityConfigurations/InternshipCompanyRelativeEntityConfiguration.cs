using Core.Database.EntityConfigurations;
using InternshipService.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternshipService.Database.EntityConfigurations
{
    /// <summary>
    /// Internship company relative entity configuration
    /// </summary>
    public class InternshipCompanyRelativeEntityConfiguration : BaseEntityConfiguration<Guid, InternshipCompanyRelative>, IInternshipCompanyRelativeEntityConfiguration
    {
        ///<inheritdoc/>
        public override void Configure(EntityTypeBuilder<InternshipCompanyRelative> builder)
        {
            base.Configure(builder);

            builder
               .HasMany(x => x.Internships)
               .WithMany(y => y.CompanyRelatives)
               .UsingEntity("Internship_Related_Person");

            builder
                .HasIndex(x => x.CompanyName)
                .IsUnique(false);

            builder
                .HasIndex(x => x.Name);

            builder
                .HasIndex(x => x.LastName);
        }
    }
}
