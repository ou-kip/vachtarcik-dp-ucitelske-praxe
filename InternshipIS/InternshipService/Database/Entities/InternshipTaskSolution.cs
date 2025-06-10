using Core.Database.Entities;

namespace InternshipService.Database.Entities
{
    /// <summary>
    /// The Internship task solution
    /// </summary>
    public class InternshipTaskSolution : BaseEntity<Guid>
    {
        /// <summary>
        /// The date and time of submittion
        /// </summary>
        public DateTime SubmittedDate { get { return CreationDate; } }

        /// <summary>
        /// The actual text solution
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// The ID of the related task
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// The related task
        /// </summary>
        public InternshipTask Task { get; set; }

        /// <summary>
        /// Files linked to the solution
        /// </summary>
        public ICollection<InternshipTaskSolutionFile>? Files { get; set; } = new List<InternshipTaskSolutionFile>();
    }
}
