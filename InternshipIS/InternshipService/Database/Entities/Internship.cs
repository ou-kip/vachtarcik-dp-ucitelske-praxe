using Core.Database.Entities;

namespace InternshipService.Database.Entities
{
    /// <summary>
    /// The entity representing the internship
    /// </summary>
    public class Internship : BaseEntity<Guid>
    {
        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The name of the company
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// The date and time when internship was selected by student
        /// </summary>
        public DateTime? SelectedOn { get; set; }

        /// <summary>
        /// The date and time when internship was finished
        /// </summary>
        public DateTime? FinishedOn { get; set; }

        /// <summary>
        /// The date and time when internship was published
        /// </summary>
        public DateTime? PublishedOn { get; set; }

        /// <summary>
        /// The date and time when internship was canceled
        /// </summary>
        public DateTime? CanceledOn { get; set; }

        /// <summary>
        /// The stae of the internship
        /// </summary>
        public int State {  get; set; } 

        /// <summary>
        /// The internship category
        /// </summary>
        public Guid? CategoryId {  get; set; }

        /// <summary>
        /// Flag indicating whether the internship is a template
        /// </summary>
        public bool IsTemplate { get; set; } = false;

        /// <summary>
        /// The related student(s)
        /// </summary>
        public ICollection<InternshipStudent> Students { get; set; } = new List<InternshipStudent>();

        /// <summary>
        /// The related teacher(s)
        /// </summary>
        public ICollection<InternshipTeacher> Teachers { get; set; } = new List<InternshipTeacher>();

        /// <summary>
        /// The company relative person(s)
        /// </summary>
        public ICollection<InternshipCompanyRelative> CompanyRelatives { get; set; } = new List<InternshipCompanyRelative>();

        /// <summary>
        /// The tasks related to the internship
        /// </summary>
        public ICollection<InternshipTask> Tasks { get; set; } = new List<InternshipTask>();

        /// <summary>
        /// The links related to the internship
        /// </summary>
        public ICollection<InternshipLink> Links { get; set; } = new List<InternshipLink>();

        /// <summary>
        /// The internship category related to the entity
        /// </summary>
        public InternshipCategory InternshipCategory { get; set; }
    }
}
