using Core.Database.Entities;

namespace InternshipService.Database.Entities
{
    /// <summary>
    /// The internship category
    /// </summary>
    public class InternshipCategory : BaseEntity<Guid>
    {
        /// <summary>
        /// The name of the category
        /// </summary>
        public string Name {  get; set; } 

        /// <summary>
        /// The Code of the category
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The Internships
        /// </summary>
        public ICollection<Internship> Internships { get; set; } = new List<Internship>();

        /// <summary>
        /// Gets the category codename
        /// </summary>
        /// <returns></returns>
        public string GetCodeName()
        {
            return string.Join("-", Code, Name);
        }
    }
}
