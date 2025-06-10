namespace Contracts
{
    public class InternshipPersonCreateNotification
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? StudentCode { get; set; }
        public string? CompanyName { get; set; }
        public InternshipPersonTypeEnum PersonType { get; set; }
    }
}
