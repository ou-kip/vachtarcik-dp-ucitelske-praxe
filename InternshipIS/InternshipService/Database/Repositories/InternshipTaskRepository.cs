using Core.Database.Repositories;
using Core.Infrastructure.Results;
using InternshipService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The repository for Internship task entities
    /// </summary>
    public class InternshipTaskRepository : BaseRepository<Guid, InternshipTask, InternshipDbContext>, IInternshipTaskRepository
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="context"></param>
        public InternshipTaskRepository(InternshipDbContext context) : base(context)
        {
        }

        ///<inheritdoc/>
        public async Task<IDataResult<InternshipTask>> GetWithFilesAsync(Guid id, CancellationToken ct)
        {
            var result = new DataResult<InternshipTask>();

            try
            {
                result.Data = await Entities
                    .Include(x => x.Files)
                    .FirstOrDefaultAsync(x => x.Id == id, ct);
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }

        ///<inheritdoc/>
        public IQueryable<InternshipTask> GetAllWithInternshipsQuery()
        {
            return Entities
                .Include(x => x.Internship)
                    .ThenInclude(i => i.Students)
                .Include(x => x.Internship)
                    .ThenInclude(i => i.Teachers)
                .Include(x => x.Internship)
                    .ThenInclude(i => i.CompanyRelatives)
                .AsQueryable();
        }
    }
}
