using Core.Database.Repositories;
using Identity.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Identity.Database.Repositories
{
    /// <summary>
    /// The user repository
    /// </summary>
    public class UserRepository : BaseRepository<Guid, User, AuthorizationDbContext>, IUserRepository
    {
        /// <summary>
        /// .ctor for UserRepository
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(AuthorizationDbContext context) : base(context)
        {
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<User>> GetUsersByEmailAsync(string email, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            return await Entities.Where(x => x.Email.Equals(email)).ToListAsync(ct);
        }
    }
}
