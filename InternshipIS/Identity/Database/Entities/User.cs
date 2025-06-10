using Core.Database.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Database.Entities
{
    /// <summary>
    /// User entity for authentication
    /// </summary>
    public class User : IdentityUser<Guid>, IEntity<Guid>
    {
        /// <summary>
        /// The name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        public string LastName {  get; set; }

        /// <summary>
        /// The user code
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// The name of the relative person company
        /// </summary>
        [NotMapped]
        public string? CompanyName {  get; set; }

        ///<inheritdoc/>
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        ///<inheritdoc/>
        public string CreationAuthor { get; set; } = "System";

        ///<inheritdoc/>
        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;

        ///<inheritdoc/>
        public string? UpdateAuthor { get; set; } = "System";
    }
}
