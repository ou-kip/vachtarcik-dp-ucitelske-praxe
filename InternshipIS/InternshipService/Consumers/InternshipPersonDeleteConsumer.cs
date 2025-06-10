using Contracts;
using Core.Infrastructure.Results;
using InternshipService.Database.Repositories;
using MassTransit;
using IResult = Core.Infrastructure.Results.IResult;

namespace InternshipService.Consumers
{
    /// <summary>
    /// Consumer which deletes the internship related person
    /// </summary>
    public class InternshipPersonDeleteConsumer : IConsumer<InternshipPersonDeleteNotification>
    {
        private readonly IInternshipStudentRepository _studentRepository;
        private readonly IInternshipTeacherRepository _teacherRepository;
        private readonly IInternshipCompanyRelativeRepository _companyRelativeRepository;

        private readonly ILogger _logger;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="studentRepository"></param>
        /// <param name="teacherRepository"></param>
        /// <param name="companyRelativeRepository"></param>
        public InternshipPersonDeleteConsumer(
            IInternshipStudentRepository studentRepository, 
            IInternshipTeacherRepository teacherRepository, 
            IInternshipCompanyRelativeRepository companyRelativeRepository,
            ILogger<InternshipPersonDeleteConsumer> logger)
        {
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
            _companyRelativeRepository = companyRelativeRepository;
            _logger = logger;
        }

        ///<inheritdoc/>
        public async Task Consume(ConsumeContext<InternshipPersonDeleteNotification> context)
        {
            var msg = context.Message;
            IResult result = new Result();

            try
            {
                if (msg.PersonType == InternshipPersonTypeEnum.Student)
                {
                    result = await _studentRepository.DeleteAsync(x=>x.Email.Equals(msg.Email));
                    await _studentRepository.SaveAsync();
                }
                else if (msg.PersonType == InternshipPersonTypeEnum.Teacher)
                {
                    result = await _teacherRepository.DeleteAsync(x => x.Email.Equals(msg.Email));
                    await _teacherRepository.SaveAsync();
                }
                else if (msg.PersonType == InternshipPersonTypeEnum.CompanyRelative)
                {
                    result = await _companyRelativeRepository.DeleteAsync(x => x.Email.Equals(msg.Email));
                    await _companyRelativeRepository.SaveAsync();
                }
                else
                {
                    _logger.LogCritical($"INTERNSHIP: Unable to solve person type. Email = {msg.Email}");
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
    }
}
