using Core.Database.Entities;

namespace InternshipService.Database.Entities
{
    /// <summary>
    /// The task related to the internship
    /// </summary>
    public class InternshipTask : BaseEntity<Guid>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public InternshipTask()
        {
        }

        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Flag indicating whether the task is completed
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Flag indicating whether the task is reported
        /// </summary>
        public bool IsReported { get; set; }

        /// <summary>
        /// The summary
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// The teachers summary
        /// </summary>
        public string TeacherSummary { get; set; }

        /// <summary>
        /// The state of the task
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// The time when the task must be finished
        /// </summary>
        public DateTime EndsOn { get; set; }

        /// <summary>
        /// The Id of related internship
        /// </summary>
        public Guid InternshipId { get; set; }

        public Internship Internship { get; set; }

        public InternshipTaskSolution? Solution { get; set; }

        public ICollection<InternshipTaskFile> Files { get; set; } = new List<InternshipTaskFile>();

        public ICollection<InternshipTaskLink> Links { get; set; } = new List<InternshipTaskLink>();
    }
}
