namespace InternshipService.Domain
{
    public class SolutionDto
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Solution { get; set; }
        public DateTime SubmittedDate { get; set; }
        public List<FileDto> Files { get; set; }
    }
}
