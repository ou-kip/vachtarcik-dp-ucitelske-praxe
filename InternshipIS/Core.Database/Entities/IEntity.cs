namespace Core.Database.Entities
{
    /// <summary>
    /// The interface for entities also working as adapter for other interfaces
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntity<T>
    {
        /// <summary>
        /// The id of the entity
        /// </summary>
        T Id { get; set; }

        /// <summary>
        /// Date and time of creation
        /// </summary>
        DateTime CreationDate { get; set; }

        /// <summary>
        /// The author of creation
        /// </summary>
        string CreationAuthor { get; set; }

        /// <summary>
        /// Date and time of update
        /// </summary>
        DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Update author
        /// </summary>
        string? UpdateAuthor { get; set; }
    }
}
