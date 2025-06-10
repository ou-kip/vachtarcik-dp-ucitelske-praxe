using Core.Database.Repositories;
using Identity.Database.Entities;

namespace Identity.Database.Repositories
{
    /// <summary>
    /// The role repository interface
    /// </summary>
    public interface IRoleRepository : IBaseRepository<Guid, Role>
    {
    }
}
