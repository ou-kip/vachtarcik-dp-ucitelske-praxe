namespace InternshipService.Database.Entities
{
    /// <summary>
    /// The student related to internship
    /// </summary>
    public class InternshipStudent : BaseInternshipPersonEntity
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="id"></param>
        public InternshipStudent(Guid id) : base(id)
        { 
        }

        /// <summary>
        /// The student code
        /// </summary>
        public string StudentCode { get; set; }

        /// <summary>
        /// The internships related to the student
        /// </summary>
        public ICollection<Internship> Internships { get; set; } = new List<Internship>();
    }
}
