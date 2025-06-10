using Core.Domain.Requests;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.Commands
{
    public class UpdateTaskCommand : BaseRequest<UpdateTaskResponse>
    {
        public UpdateTaskCommand() 
        { 
        }

        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime EndsOn { get; set; }

        public bool IsReported { get; set; }

        public int State { get; set; }

        public string Summary { get; set; }

        public string TeacherSummary { get; set; }

        public List<LinkDto>? Links { get; set; }
    }
}
