using Core.Database.Repositories;
using InternshipService.Database.Entities;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The internship link repository
    /// </summary>
    public class InternshipLinkRepository : BaseRepository<Guid, InternshipLink, InternshipDbContext>, IInternshipLinkRepository
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="context"></param>
        public InternshipLinkRepository(InternshipDbContext context) : base(context)
        {
        }
    }
}
