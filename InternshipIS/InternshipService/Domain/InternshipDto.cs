namespace InternshipService.Domain
{
    public class InternshipDto
    {
        /// <summary>
        /// The Id
        /// </summary>
        public Guid Id { get; set; }

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
        /// The date and time when internship was created
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The date and time when internship was ended
        /// </summary>
        public DateTime? EndsOn { get; set; }

        /// <summary>
        /// The www links related to the internship
        /// </summary>
        public List<LinkDto>? Links { get; set; }

        /// <summary>
        /// The stae of the internship
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// The id of the student
        /// </summary>
        public StudentDto? Student {  get; set; }

        /// <summary>
        /// The teachers ids
        /// </summary>
        public List<TeacherDto>? Teachers { get; set; }

        /// <summary>
        /// The company relatives ids
        /// </summary>
        public List<CompanyRelativeDto>? CompanyRelatives { get; set; }

        /// <summary>
        /// The category id
        /// </summary>
        public CategoryDto Category {  get; set; }

        /// <summary>
        /// The id of the author
        /// </summary>
        public string CreatedByName { get; set; }

        /// <summary>
        /// Flag indicating whether the request author is also author of the internship
        /// </summary>
        public bool IsCreatedByRequester { get; set; }
    }
}
