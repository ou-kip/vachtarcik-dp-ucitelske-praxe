using Core.Database.Repositories;
using InternshipService.Database.Entities;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The internship task link repository
    /// </summary>
    public class InternshipTaskLinkRepository : BaseRepository<Guid, InternshipTaskLink, InternshipDbContext>, IInternshipTaskLinkRepository
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="context"></param>
        public InternshipTaskLinkRepository(InternshipDbContext context) : base(context)
        {
        }
    }
}
