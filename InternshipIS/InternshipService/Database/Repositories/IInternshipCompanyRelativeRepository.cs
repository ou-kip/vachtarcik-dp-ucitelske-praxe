using Core.Database.Repositories;
using InternshipService.Database.Entities;

namespace InternshipService.Database.Repositories
{
    /// <summary>
    /// The internship company relative repository interface
    /// </summary>
    public interface IInternshipCompanyRelativeRepository : IBaseRepository<Guid, InternshipCompanyRelative>
    {
        /// <summary>
        /// Gets collection of InternshipCompanyRelative by name of the company
        /// </summary>
        /// <param name="companyName"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<IList<InternshipCompanyRelative>> GetCollectionByCompanyNameAsync(string companyName = "", CancellationToken ct = default);

        /// <summary>
        /// Gets all company names
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<IList<string>> GetCompaniesCollectionAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets the company relative by email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<InternshipCompanyRelative> GetByEmailAsync(string email, CancellationToken ct);
    }
}
