using Core.Database.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Database.Entities
{
    /// <summary>
    /// The role entity for authorization
    /// </summary>
    public class Role : IdentityRole<Guid>, IEntity<Guid>
    {
        ///<inheritdoc/>
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        ///<inheritdoc/>
        public string CreationAuthor { get; set; }

        ///<inheritdoc/>
        public DateTime? UpdatedDate { get; set; }

        ///<inheritdoc/>
        public string? UpdateAuthor { get; set; }
    }
}
