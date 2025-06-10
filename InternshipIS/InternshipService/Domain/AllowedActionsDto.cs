namespace InternshipService.Domain
{
    public class AllowedActionsDto
    {
        public bool Allowed { get; set; } = false;
        public bool IsStudent { get; set; } = false;
        public bool IsTeacher { get; set; } = false;
        public bool IsCompanyRelative { get; set; } = false;
    }
}
