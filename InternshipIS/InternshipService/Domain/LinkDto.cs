namespace InternshipService.Domain
{
    /// <summary>
    /// The dto for InternshipLink entity
    /// </summary>
    public class LinkDto
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public LinkDto() { }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="url"></param>
        public LinkDto(Guid id, string name, string url) 
        {
            Id = id;
            Name = name;
            Url = url;
        }

        /// <summary>
        /// The id of the link
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// The name of the link
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The url of the link
        /// </summary>
        public string Url { get; set; }
    }
}
