using Core.Database.Entities;

namespace InternshipService.Database.Entities
{
    /// <summary>
    /// The base class for internship related persons
    /// </summary>
    public abstract class BaseInternshipPersonEntity : BaseEntity<Guid>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="id"></param>
        public BaseInternshipPersonEntity(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// The user id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The email
        /// </summary>
        public string Email { get; set; }
    }
}
