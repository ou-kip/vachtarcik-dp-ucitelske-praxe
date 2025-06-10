using Core.Database.Repositories;
using InternshipService.Database.Entities;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The Internship task solution file repository interface
    /// </summary>
    public interface IInternshipTaskSolutionFileRepository : IBaseRepository<Guid, InternshipTaskSolutionFile>
    {
    }
}
