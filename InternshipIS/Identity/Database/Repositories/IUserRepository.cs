using Core.Database.Repositories;
using Identity.Database.Entities;

namespace Identity.Database.Repositories
{
    /// <summary>
    /// The user repository interface
    /// </summary>
    public interface IUserRepository : IBaseRepository<Guid, User>
    {
        /// <summary>
        /// Try get user(s) by a specific email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IEnumerable<User>> GetUsersByEmailAsync(string email, CancellationToken ct = default);
    }
}
