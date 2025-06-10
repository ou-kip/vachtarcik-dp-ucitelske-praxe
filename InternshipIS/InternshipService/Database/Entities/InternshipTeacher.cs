
namespace InternshipService.Database.Entities
{
    /// <summary>
    /// The teacher related to internship
    /// </summary>
    public class InternshipTeacher : BaseInternshipPersonEntity
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="id"></param>
        public InternshipTeacher(Guid id) : base(id)
        {
        }

        public ICollection<Internship> Internships { get; set; } = new List<Internship>();
    }
}
