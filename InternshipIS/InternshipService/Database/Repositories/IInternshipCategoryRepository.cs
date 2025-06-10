using Core.Database.Repositories;
using InternshipService.Database.Entities;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The internship category repository interface
    /// </summary>
    public interface IInternshipCategoryRepository : IBaseRepository<Guid, InternshipCategory>
    {
    }
}
