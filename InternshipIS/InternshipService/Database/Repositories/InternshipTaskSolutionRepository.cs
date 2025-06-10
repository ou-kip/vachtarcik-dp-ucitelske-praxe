using Core.Database.Repositories;
using Core.Infrastructure.Results;
using InternshipService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The internship task solution repository
    /// </summary>
    public class InternshipTaskSolutionRepository : BaseRepository<Guid, InternshipTaskSolution, InternshipDbContext>, IInternshipTaskSolutionRepository
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="context"></param>
        public InternshipTaskSolutionRepository(InternshipDbContext context) : base(context)
        {
        }

        ///<inheritdoc/>
        public async Task<IDataResult<InternshipTaskSolution>> GetByTaskIdAsync(Guid taskId, CancellationToken ct)
        {
            var result = new DataResult<InternshipTaskSolution>();

            try
            {
                result.Data = await Entities.FirstOrDefaultAsync(x => x.TaskId == taskId, ct);
            }
            catch (Exception ex) 
            {
                result.AddError(ex);
            }

            return result;
        }

        ///<inheritdoc/>
        public async Task<IDataResult<InternshipTaskSolution>> GetWithFilesByTaskIdAsync(Guid taskId, CancellationToken ct)
        {
            var result = new DataResult<InternshipTaskSolution>();

            try
            {
                result.Data = await Entities
                    .Include(x => x.Files)
                    .FirstOrDefaultAsync(x => x.TaskId == taskId, ct);
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }
    }
}
