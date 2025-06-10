using Core.Database.Repositories;
using InternshipService.Database.Entities;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The internship task solution file repository
    /// </summary>
    public class InternshipTaskSolutionFileRepository : BaseRepository<Guid, InternshipTaskSolutionFile, InternshipDbContext>, IInternshipTaskSolutionFileRepository
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="context"></param>
        public InternshipTaskSolutionFileRepository(InternshipDbContext context) : base(context)
        {
        }
    }
}
