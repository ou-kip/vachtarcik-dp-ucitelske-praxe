using Core.Database.Repositories;
using InternshipService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The internship company relative repository
    /// </summary>
    public class InternshipCompanyRelativeRepository : BaseRepository<Guid, InternshipCompanyRelative, InternshipDbContext>, IInternshipCompanyRelativeRepository
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="context"></param>
        public InternshipCompanyRelativeRepository(InternshipDbContext context) : base(context)
        {
        }

        ///<inheritdoc/>
        public async Task<IList<InternshipCompanyRelative>> GetCollectionByCompanyNameAsync(string companyName = "", CancellationToken ct = default)
        {
            return await Entities
                .Where(x => x.CompanyName.Equals(companyName))
                .ToListAsync(ct);
        }

        ///<inheritdoc/>
        public async Task<IList<string>> GetCompaniesCollectionAsync(CancellationToken ct = default)
        {
            return await Entities
                .Select(x => x.CompanyName)
                .Distinct()
                .OrderBy(x=>x)
                .ToListAsync(ct);
        }

        ///<inheritdoc/>
        public async Task<InternshipCompanyRelative> GetByEmailAsync(string email, CancellationToken ct)
        {
            return await Entities.FirstOrDefaultAsync(x => x.Email.Equals(email));
        }
    }
}
