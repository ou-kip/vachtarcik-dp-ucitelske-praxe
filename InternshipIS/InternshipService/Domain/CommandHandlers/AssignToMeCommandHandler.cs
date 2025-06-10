using Contracts;
using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Configuration;
using InternshipService.Database.Entities;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Enums;
using InternshipService.Domain.Responses;
using MassTransit;
using Microsoft.Extensions.Options;
using System.Text;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for assigning the internship to the logged in person
    /// </summary>
    public class AssignToMeCommandHandler : BaseInternshipCommandHandler<AssignToMeCommand, AssignToMeResponse>
    {
        private readonly IPublishEndpoint _publisher;
        private readonly ClientOptions _clientOptions;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="internshipRepository"></param>
        /// <param name="internshipTeacherRepository"></param>
        /// <param name="internshipStudentRepository"></param>
        /// <param name="internshipCompanyRelativeRepository"></param>
        /// <param name="internshipCategoryRepository"></param>
        /// <param name="internshipLinkRepository"></param>
        /// <param name="publisher"></param>
        /// <param name="clientOptions"></param>
        /// <param name="logger"></param>
        public AssignToMeCommandHandler(
            IInternshipRepository internshipRepository, 
            IInternshipTeacherRepository internshipTeacherRepository, 
            IInternshipStudentRepository internshipStudentRepository, 
            IInternshipCompanyRelativeRepository internshipCompanyRelativeRepository, 
            IInternshipCategoryRepository internshipCategoryRepository,
            IInternshipLinkRepository internshipLinkRepository,
            IPublishEndpoint publisher,
            IOptions<ClientOptions> clientOptions,
            ILogger<AssignToMeCommandHandler> logger) : base(internshipRepository, internshipTeacherRepository, internshipStudentRepository, internshipCompanyRelativeRepository, internshipCategoryRepository, internshipLinkRepository, logger)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _clientOptions = clientOptions.Value ?? throw new ArgumentNullException(nameof(clientOptions));
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<AssignToMeResponse>> HandleAsync(AssignToMeCommand request, CancellationToken ct)
        {
            var result = new ApiResult<AssignToMeResponse>();

            var internshipResult = await InternshipRepository.GetAsync(request.InternshipId, ct);
            if (internshipResult.HasErrors || internshipResult.Data == null)
            {
                throw new NotFoundException(typeof(Internship).Name, request.InternshipId.ToString());
            }

            var studentResult = await InternshipStudentRepository.GetByEmailAsync(request.PerformerEmail, ct);
            if (studentResult.HasErrors || studentResult.Data == null)
            {
                throw new NotFoundException(typeof(InternshipStudent).Name, request.PerformerEmail);
            }

            var entity = internshipResult.Data;
            entity.SelectedOn = DateTime.Now;
            entity.State = (int)InternshipStateEnum.Chosen;

            await AddStudentsAsync(entity, studentResult.Data.Id, ct);

            InternshipRepository.Update(entity);
            await InternshipRepository.SaveAsync(ct);

            result.Data.Assigned = true;
            result.Data.StatusCode = StatusCodes.Status200OK;
            result.StatusCode = StatusCodes.Status200OK;

            await SendEmailMessageAsync(entity, studentResult.Data, ct);

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(AssignToMeCommand request, CancellationToken ct)
        {
            if (request.InternshipId == null || request.InternshipId == Guid.Empty)
            {
                throw new ArgumentNullException("Internship Id must be filled.");
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Sends the email message via notification service
        /// </summary>
        /// <param name="internship"></param>
        /// <param name="student"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task SendEmailMessageAsync(Internship internship, InternshipStudent student, CancellationToken ct)
        {
            var receivers = internship.Teachers
                .Select(x => x.Email)
                .ToList();

            var internshipId = internship.Id.ToString();

            var builder = new StringBuilder();
            builder.AppendLine($"Praxe {internship.Name} byla vybrána studentem: ");
            builder.AppendLine($"Studijní číslo: {student.StudentCode}");
            builder.AppendLine($"Jméno studenta: {student.Name} {student.LastName}");
            builder.AppendLine();
            builder.AppendLine($"Odkaz na praxi: {_clientOptions.Url}/internship/edit?internshipId={internshipId}");

            var notification = new EmailNotification()
            {
                Receivers = receivers,
                Subject = $"Praxe {internship.Name} byla vybrána studentem.",
                Message = builder.ToString()
            };

            await _publisher.Publish( notification, ct);
        }
    }
}
