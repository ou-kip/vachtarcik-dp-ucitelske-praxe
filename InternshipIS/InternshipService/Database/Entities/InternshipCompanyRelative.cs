
namespace InternshipService.Database.Entities
{
    /// <summary>
    /// The company relative person related to internship
    /// </summary>
    public class InternshipCompanyRelative : BaseInternshipPersonEntity
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="id"></param>
        public InternshipCompanyRelative(Guid id) : base(id)
        {
        }

        /// <summary>
        /// The name of the company
        /// </summary>
        public string CompanyName { get; set; }

        public ICollection<Internship> Internships { get; set; } = new List<Internship>();
    }
}
