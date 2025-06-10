using Core.Database.Repositories;
using InternshipService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The internship teacher repository
    /// </summary>
    public class InternshipTeacherRepository : BaseRepository<Guid, InternshipTeacher, InternshipDbContext>, IInternshipTeacherRepository
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="context"></param>
        public InternshipTeacherRepository(InternshipDbContext context) : base(context)
        {
        }

        ///<inheritdoc/>
        public async Task<InternshipTeacher> GetByEmailAsync(string email, CancellationToken ct)
        {
            return await Entities.FirstOrDefaultAsync(x => x.Email.Equals(email), ct);
        }
    }
}
