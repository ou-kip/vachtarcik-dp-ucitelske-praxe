namespace InternshipService.Domain
{
    public class RichInternshipTaskDto : InternshipTaskDto
    {
        public RichInternshipTaskDto() { }

        public Guid InternshipId { get; set; }
        public string InternshipName { get; set; }
    }
}
