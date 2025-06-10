using Core.Database.Repositories;
using InternshipService.Database.Entities;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The internship teacher repository interface
    /// </summary>
    public interface IInternshipTeacherRepository : IBaseRepository<Guid, InternshipTeacher>
    {
        /// <summary>
        /// Gets the teacher by email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<InternshipTeacher> GetByEmailAsync(string email, CancellationToken ct);
    }
}
