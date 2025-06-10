using Core.Database.Repositories;
using InternshipService.Database.Entities;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The Internship task link repository interface
    /// </summary>
    public interface IInternshipTaskLinkRepository : IBaseRepository<Guid, InternshipTaskLink>
    {
    }
}
