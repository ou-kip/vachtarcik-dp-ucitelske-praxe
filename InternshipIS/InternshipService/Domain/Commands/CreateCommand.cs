using Core.Domain.Requests;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.Commands
{
    /// <summary>
    /// The command for creating new internship
    /// </summary>
    public class CreateCommand : BaseRequest<CreateResponse>
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? StudentId { get; set; }
        public List<Guid>? TeacherIds { get; set; }
        public List<Guid>? CompanyRelativeIds { get; set; }
        public List<LinkDto> Links { get; set; }
        public DateTime? EndsOn {  get; set; }
        public int State { get; set; }
    }
}
