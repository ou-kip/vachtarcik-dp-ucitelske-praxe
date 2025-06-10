using Core.Domain.Requests;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.Commands
{
    /// <summary>
    /// The command for updating the internship
    /// </summary>
    public class UpdateCommand : BaseRequest<UpdateResponse>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="companyName"></param>
        /// <param name="endsOn"></param>
        /// <param name="links"></param>
        /// <param name="teachers"></param>
        /// <param name="students"></param>
        /// <param name="companyRelatives"></param>
        /// <param name="state"></param>
        public UpdateCommand(
            Guid id,
            string name, 
            string description, 
            string companyName,
            DateTime? endsOn,
            List<LinkDto> links,
            List<TeacherDto> teachers,
            List<StudentDto> students,
            List<CompanyRelativeDto> companyRelatives,
            Guid categoryId,
            int state) 
        {
            Id = id;
            Name = name;
            Description = description;
            CompanyName = companyName;
            EndsOn = endsOn;
            State = state;
            Links = links;
            Teachers = teachers;
            Students = students;
            CompanyRelatives = companyRelatives;
            CategoryId = categoryId;
            State = state;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string CompanyName { get; }
        public DateTime? EndsOn { get; }
        public List<LinkDto> Links { get; }
        public List<TeacherDto> Teachers { get; }
        public List<StudentDto> Students { get; }
        public List<CompanyRelativeDto> CompanyRelatives { get; }
        public Guid CategoryId { get; }
        public int State { get; }
    }
}
