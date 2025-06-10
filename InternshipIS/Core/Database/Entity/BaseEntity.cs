namespace Core.Database.Entity
{
    /// <summary>
    /// The base entity
    /// </summary>
    public abstract class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; }

        public string CreationAuthor { get; set; }

        public DateTime CreationDate { get; set; }

        public string UpdateAuthor { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
