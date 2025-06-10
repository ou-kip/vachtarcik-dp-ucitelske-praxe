using Core.Database.Entities;

namespace InternshipService.Database.Entities
{
    /// <summary>
    /// The link related to the internship
    /// </summary>
    public class InternshipLink : BaseEntity<Guid>
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
        public Guid InternshipId { get; set; }

        public Internship Internship { get; set; }
    }
}
