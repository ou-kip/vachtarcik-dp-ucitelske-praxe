using Contracts;
using Core.Constants;
using Core.Infrastructure.Results;
using InternshipService.Database.Entities;
using InternshipService.Database.Repositories;
using MassTransit;
using IResult = Core.Infrastructure.Results.IResult;

namespace InternshipService.Consumers
{
    /// <summary>
    /// The consumer which creates Internship related person
    /// </summary>
    public class InternshipPersonCreateConsumer : IConsumer<InternshipPersonCreateNotification>
    {
        private readonly IInternshipStudentRepository _studentRepository;
        private readonly IInternshipTeacherRepository _teacherRepository;
        private readonly IInternshipCompanyRelativeRepository _companyRelativeRepository;
        
        private readonly ILogger _logger;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="studentRepository"></param>
        /// <param name="logger"></param>
        public InternshipPersonCreateConsumer(
            IInternshipStudentRepository studentRepository,
            IInternshipTeacherRepository teacherRepository,
            IInternshipCompanyRelativeRepository companyRelativeRepository,
            ILogger<InternshipPersonCreateConsumer> logger)
        {
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
            _companyRelativeRepository = companyRelativeRepository;
            _logger = logger;
        }

        ///<inheritdoc/>
        public async Task Consume(ConsumeContext<InternshipPersonCreateNotification> context)
        {
            var msg = context.Message;
            IResult result = new Result();

            try
            {
                if (msg.PersonType == InternshipPersonTypeEnum.Student)
                {
                    result = await CreateStudentAsync(msg);
                }
                else if (msg.PersonType == InternshipPersonTypeEnum.Teacher)
                {
                    result = await CreateTeacherAsync(msg);
                }
                else if (msg.PersonType == InternshipPersonTypeEnum.CompanyRelative)
                {
                    result = await CreateCompanyRelativeAsync(msg);
                }
                else
                {
                    _logger.LogCritical($"INTERNSHIP: Unable to solve person type. UserId = {msg.UserId}");
                }

                if (result.HasErrors)
                {
                    _logger.LogError(result.GetErrorMessage());
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Create internship student entity 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task<IResult> CreateStudentAsync(InternshipPersonCreateNotification msg, CancellationToken ct = default)
        {
            var student = new InternshipStudent(Guid.NewGuid())
            {
                UserId = msg.UserId,
                Email = msg.Email,
                LastName = msg.LastName,
                Name = msg.Name,
                StudentCode = msg.StudentCode,
                CreationAuthor = SystemConstants.SystemUser,
                CreationDate = DateTime.Now,
                UpdateAuthor = SystemConstants.SystemUser,
                UpdatedDate = DateTime.Now
            };

            await _studentRepository.InsertAsync(student);
            var result = await _studentRepository.SaveAsync();

            return result;
        }

        /// <summary>
        /// Create internship teacher entity 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task<IResult> CreateTeacherAsync(InternshipPersonCreateNotification msg, CancellationToken ct = default)
        {
            var teacher = new InternshipTeacher(Guid.NewGuid())
            {
                UserId = msg.UserId,
                Email = msg.Email,
                LastName = msg.LastName,
                Name = msg.Name,
                CreationAuthor = SystemConstants.SystemUser,
                CreationDate = DateTime.Now,
                UpdateAuthor = SystemConstants.SystemUser,
                UpdatedDate = DateTime.Now
            };

            await _teacherRepository.InsertAsync(teacher);
            var result = await _teacherRepository.SaveAsync();

            return result;
        }

        /// <summary>
        /// Create internship company relative entity 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task<IResult> CreateCompanyRelativeAsync(InternshipPersonCreateNotification msg, CancellationToken ct = default)
        {
            var companyRelative = new InternshipCompanyRelative(Guid.NewGuid())
            {
                UserId = msg.UserId,
                Email = msg.Email,
                LastName = msg.LastName,
                Name = msg.Name,
                CompanyName = msg.CompanyName,
                CreationAuthor = SystemConstants.SystemUser,
                CreationDate = DateTime.Now,
                UpdateAuthor = SystemConstants.SystemUser,
                UpdatedDate = DateTime.Now
            };

            await _companyRelativeRepository.InsertAsync(companyRelative);
            var result = await _companyRelativeRepository.SaveAsync();

            return result;
        }
    }
}
