using Core.Database.Entities;

namespace InternshipService.Database.Entities
{
    public class InternshipTaskSolutionFile : BaseEntity<Guid>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public InternshipTaskSolutionFile() { }

        /// <summary>
        /// The name of the file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The name of the file without extension
        /// </summary>
        public string FileNameWithoutExt { get; set; }

        /// <summary>
        /// The file extension
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// The content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The id of the related task
        /// </summary>
        public Guid SolutionId { get; set; }
        public InternshipTaskSolution Solution { get; set; }
    }
}
