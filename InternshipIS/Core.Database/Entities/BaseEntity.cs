namespace Core.Database.Entities
{
    public abstract class BaseEntity<TKey> : IEntity<TKey>
    {
        ///<inheritdoc/>
        public TKey Id { get; set; }

        /// <summary>
        /// The author of creation
        /// </summary>
        public string CreationAuthor { get; set; }

        /// <summary>
        /// The date when created
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The author of update
        /// </summary>
        public string? UpdateAuthor { get; set; }

        /// <summary>
        /// The update date
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
    }
}
