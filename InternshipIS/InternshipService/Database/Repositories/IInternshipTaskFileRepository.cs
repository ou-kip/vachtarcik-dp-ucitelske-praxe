using Core.Database.Repositories;
using Core.Infrastructure.Results;
using InternshipService.Database.Entities;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The Internship task file repository interface
    /// </summary>
    public interface IInternshipTaskFileRepository : IBaseRepository<Guid, InternshipTaskFile>
    {
        /// <summary>
        /// Get the files by taskId
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IDataResult<List<InternshipTaskFile>>> GetByTaskIdAsync(Guid taskId, CancellationToken ct);
    }
}
