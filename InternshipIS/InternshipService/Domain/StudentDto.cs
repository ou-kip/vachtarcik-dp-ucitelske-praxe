using System.Text.Json.Serialization;

namespace InternshipService.Domain
{
    public class StudentDto
    {
        /// <summary>
        /// .ctor
        /// </summary>
        [JsonConstructor]
        public StudentDto() { }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        public StudentDto(Guid id, string name, string lastName)
        {
            Id = id;
            FullName = string.Join(" ", name, lastName);
        }

        /// <summary>
        /// The Id of the teacher
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The students full name
        /// </summary>
        public string FullName { get; set; }
    }
}
