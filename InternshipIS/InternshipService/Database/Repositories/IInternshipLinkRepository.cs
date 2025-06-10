using Core.Database.Repositories;
using InternshipService.Database.Entities;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The internship link repository interface
    /// </summary>
    public interface IInternshipLinkRepository : IBaseRepository<Guid, InternshipLink>
    {
    }
}
