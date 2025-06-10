using Core.Database.Entities;

namespace InternshipService.Database.Entities
{
    public class InternshipTaskLink : BaseEntity<Guid>
    {
        /// <summary>
        /// The name of the link
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The url of the link
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The Id of the internship
        /// </summary>
        public Guid TaskId { get; set; }

        public InternshipTask Task { get; set; }
    }
}
