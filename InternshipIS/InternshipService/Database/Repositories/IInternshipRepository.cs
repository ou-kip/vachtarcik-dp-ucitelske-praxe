using Core.Database.Repositories;
using Core.Infrastructure.Results;
using InternshipService.Database.Entities;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The internship repository interface
    /// </summary>
    public interface IInternshipRepository : IBaseRepository<Guid, Internship>
    {
        /// <summary>
        /// Gets the internship also with the tasks
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IDataResult<Internship>> GetWithTasksByIdAsync(Guid id, CancellationToken ct);

        /// <summary>
        /// Gets the internship with all related data including tasks, students, teachers, and company relatives.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IDataResult<Internship>> GetWithAllDataByIdAsync(Guid id, CancellationToken ct);
    }
}
