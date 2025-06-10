using Core.Database.Repositories;
using Core.Infrastructure.Results;
using InternshipService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The internship student repository
    /// </summary>
    public class InternshipStudentRepository : BaseRepository<Guid, InternshipStudent, InternshipDbContext>, IInternshipStudentRepository
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="context"></param>
        public InternshipStudentRepository(InternshipDbContext context) : base(context)
        {
        }

        ///<inheritdoc/>
        public async Task<IDataResult<InternshipStudent>> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            var result = new DataResult<InternshipStudent>();

            try
            {
                result.Data = await Entities.SingleOrDefaultAsync(x => x.Email == email, ct);
            }
            catch (Exception ex) 
            {
                result.AddError(ex);
            }

            return result;
        }
    }
}
