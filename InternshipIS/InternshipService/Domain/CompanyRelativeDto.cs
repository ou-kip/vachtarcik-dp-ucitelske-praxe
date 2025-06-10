using System.Text.Json.Serialization;

namespace InternshipService.Domain
{
    /// <summary>
    /// Data transfer object for InternshipCompanyRelative
    /// </summary>
    public class CompanyRelativeDto
    {
        /// <summary>
        /// .ctor
        /// </summary>
        [JsonConstructor]
        public CompanyRelativeDto() { }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        public CompanyRelativeDto(Guid id, string name, string lastName)
        {
            Id = id;
            FullName = string.Join(" ", name, lastName);
        }

        /// <summary>
        /// The id of InternshipCompanyRelative
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name and last name of InternshipCompanyRelative
        /// </summary>
        public string FullName { get; set; }
    }
}
