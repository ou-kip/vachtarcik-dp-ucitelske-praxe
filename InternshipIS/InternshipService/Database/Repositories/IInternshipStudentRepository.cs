using Core.Database.Repositories;
using Core.Infrastructure.Results;
using InternshipService.Database.Entities;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The internship student repository interface
    /// </summary>
    public interface IInternshipStudentRepository : IBaseRepository<Guid, InternshipStudent>
    {
        /// <summary>
        /// Gets a student by students email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IDataResult<InternshipStudent>> GetByEmailAsync(string email, CancellationToken ct = default);
    }
}
