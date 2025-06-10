using Core.Database.Repositories;
using Core.Infrastructure.Results;
using InternshipService.Database.Entities;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The repository interface for Internship task entities
    /// </summary>
    public interface IInternshipTaskRepository : IBaseRepository<Guid, InternshipTask>
    {
        /// <summary>
        /// Gets the internship task with files
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IDataResult<InternshipTask>> GetWithFilesAsync(Guid id, CancellationToken ct);

        /// <summary>
        /// Gets the internship tasks with their internships query
        /// </summary>
        /// <returns></returns>
        IQueryable<InternshipTask> GetAllWithInternshipsQuery();
    }
}
