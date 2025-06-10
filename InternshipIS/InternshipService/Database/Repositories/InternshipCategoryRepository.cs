using Core.Database.Repositories;
using InternshipService.Database.Entities;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The internship category repository
    /// </summary>
    public class InternshipCategoryRepository : BaseRepository<Guid, InternshipCategory, InternshipDbContext>, IInternshipCategoryRepository
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="context"></param>
        public InternshipCategoryRepository(InternshipDbContext context) : base(context)
        {
        }
    }
}
