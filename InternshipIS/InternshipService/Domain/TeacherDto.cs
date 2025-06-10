using System.Text.Json.Serialization;

namespace InternshipService.Domain
{
    /// <summary>
    /// The dto for teacher
    /// </summary>
    public class TeacherDto
    {
        /// <summary>
        /// .ctor
        /// </summary>
        [JsonConstructor]
        public TeacherDto() { }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        public TeacherDto(Guid id, string name, string lastName)
        {
            Id = id;
            FullName = string.Join(" ", name, lastName);
        }

        /// <summary>
        /// The Id of the teacher
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The teachers full name
        /// </summary>
        public string FullName { get; set; }
    }
}
