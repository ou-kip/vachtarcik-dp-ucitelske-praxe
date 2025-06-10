using Core.Database.Repositories;
using Core.Infrastructure.Results;
using InternshipService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The internship task file repository
    /// </summary>
    public class InternshipTaskFileRepository : BaseRepository<Guid, InternshipTaskFile, InternshipDbContext>, IInternshipTaskFileRepository
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="context"></param>
        public InternshipTaskFileRepository(InternshipDbContext context) : base(context)
        {
        }

        ///<inheritdoc/>
        public async Task<IDataResult<List<InternshipTaskFile>>> GetByTaskIdAsync(Guid taskId, CancellationToken ct)
        {
            var result = new DataResult<List<InternshipTaskFile>>();

            try
            {
                result.Data = await Entities
                    .Where(x => x.TaskId == taskId)
                    .ToListAsync(ct);
            }
            catch (Exception ex) 
            { 
                result.AddError(ex);
            }

            return result;
        }
    }
}
