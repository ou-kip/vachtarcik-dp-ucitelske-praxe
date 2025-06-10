using Core.Database.Repositories;
using Identity.Database.Entities;

namespace Identity.Database.Repositories
{
    /// <summary>
    /// The role repository
    /// </summary>
    public class RoleRepository : BaseRepository<Guid, Role, AuthorizationDbContext>, IRoleRepository
    {
        /// <summary>
        /// .ctor for RoleRepository
        /// </summary>
        /// <param name="context"></param>
        public RoleRepository(AuthorizationDbContext context) : base(context)
        {
        }
    }
}
