using Core.Database.Repositories;
using Core.Infrastructure.Results;
using InternshipService.Database.Entities;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The Internship task solution repository interface
    /// </summary>
    public interface IInternshipTaskSolutionRepository : IBaseRepository<Guid, InternshipTaskSolution>
    {
        /// <summary>
        /// Gets the solution by given task ID
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IDataResult<InternshipTaskSolution>> GetByTaskIdAsync(Guid taskId, CancellationToken ct);

        /// <summary>
        /// Gets the solution with files by given task ID
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IDataResult<InternshipTaskSolution>> GetWithFilesByTaskIdAsync(Guid taskId, CancellationToken ct);
    }
}
