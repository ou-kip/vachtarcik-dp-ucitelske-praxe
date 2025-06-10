using Core.Database.Repositories;
using Core.Infrastructure.Results;
using InternshipService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The internship repository
    /// </summary>
    public class InternshipRepository : BaseRepository<Guid, Internship, InternshipDbContext>, IInternshipRepository
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="context"></param>
        public InternshipRepository(InternshipDbContext context) : base(context)
        {
        }

        ///<inheritdoc/>
        public async Task<IDataResult<Internship>> GetWithTasksByIdAsync(Guid id, CancellationToken ct)
        {
            var result = new DataResult<Internship>();

            try
            {
                result.Data = await Entities
                    .Include(x => x.Tasks)
                    .FirstOrDefaultAsync(x => x.Id == id, ct);
            }
            catch (Exception ex) 
            {
                result.AddError(ex);
            }

            return result;
        }

        ///<inheritdoc/>
        public async Task<IDataResult<Internship>> GetWithAllDataByIdAsync(Guid id, CancellationToken ct)
        {
            var result = new DataResult<Internship>();

            try
            {
                result.Data = await Entities
                    .Include(x => x.Tasks)
                    .Include(x => x.Students)
                    .Include(x => x.Teachers)
                    .Include(x => x.CompanyRelatives)
                    .FirstOrDefaultAsync(x => x.Id == id, ct);
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }
    }
}
