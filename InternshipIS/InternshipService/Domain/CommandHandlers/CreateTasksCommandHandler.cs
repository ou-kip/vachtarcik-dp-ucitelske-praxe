using Contracts;
using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Configuration;
using InternshipService.Database.Entities;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;
using MassTransit;
using Microsoft.Extensions.Options;
using System.Text;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for creating the tasks
    /// </summary>
    public class CreateTasksCommandHandler : BaseInternshipCommandHandler<CreateTasksCommand, CreateTasksResponse>
    {
        private readonly IInternshipTaskRepository _internshipTaskRepository;
        private readonly IInternshipTaskLinkRepository _internshipTaskLinkRepository;
        private readonly IPublishEndpoint _publisher;
        private readonly ClientOptions _clientOptions;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="internshipTaskRepository"></param>
        /// <param name="internshipTaskLinkRepository"></param>
        /// <param name="internshipRepository"></param>
        /// <param name="publisher"></param>
        /// <param name="clientOptions"></param>
        /// <param name="logger"></param>
        public CreateTasksCommandHandler(
            IInternshipTaskRepository internshipTaskRepository,
            IInternshipTaskLinkRepository internshipTaskLinkRepository,
            IInternshipRepository internshipRepository,
            IInternshipTeacherRepository internshipTeacherRepository,
            IInternshipStudentRepository internshipStudentRepository,
            IInternshipCompanyRelativeRepository internshipCompanyRelativeRepository,
            IInternshipCategoryRepository internshipCategoryRepository,
            IInternshipLinkRepository internshipLinkRepository,
            IPublishEndpoint publisher,
            IOptions<ClientOptions> clientOptions,
            ILogger<CreateTasksCommandHandler> logger) : 
            base(internshipRepository, internshipTeacherRepository, internshipStudentRepository, internshipCompanyRelativeRepository, internshipCategoryRepository, internshipLinkRepository, logger)
        {
            _internshipTaskRepository = internshipTaskRepository;
            _internshipTaskLinkRepository = internshipTaskLinkRepository;
            _publisher = publisher;
            _clientOptions = clientOptions.Value;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<CreateTasksResponse>> HandleAsync(CreateTasksCommand request, CancellationToken ct)
        {
            var result = new ApiResult<CreateTasksResponse>();
            var entities = new List<InternshipTask>();

            var internshipResult = await InternshipRepository.GetAsync(request.InternshipId, ct);
            if (internshipResult.HasErrors || internshipResult.Data == null)
            {
                throw new NotFoundException(typeof(Internship).Name, request.Id.ToString());
            }

            var entity = new InternshipTask()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                EndsOn = request.EndsOn,
                InternshipId = request.InternshipId,
                IsCompleted = request.IsCompleted,
                CreationAuthor = request.PerformerEmail,
                CreationDate = DateTime.Now,
                UpdateAuthor = request.PerformerEmail,
                UpdatedDate = DateTime.Now,
                State = request.State,
                IsReported = request.IsReported,
                Summary = request.Summary,
                TeacherSummary = request.TeacherSummary,
            };

            entities.Add(entity);

            await _internshipTaskRepository.InsertAsync(entity, ct);
            await _internshipTaskRepository.SaveAsync(ct);

            if (request.Links != null && request.Links.Any()) 
            {
                foreach (var link in request.Links)
                {
                    if (string.IsNullOrEmpty(link.Name)) { continue; }
                    if (string.IsNullOrEmpty(link.Url)) { continue; }

                    var linkEntity = new InternshipTaskLink
                    {
                        Id = Guid.NewGuid(),
                        Name = link.Name,
                        Url = link.Url,
                        CreationAuthor = request.PerformerEmail,
                        CreationDate = DateTime.Now,
                        UpdateAuthor = request.PerformerEmail,
                        UpdatedDate = DateTime.Now,
                        TaskId = entity.Id,
                    };

                    await _internshipTaskLinkRepository.InsertAsync(linkEntity, ct);
                }

                await _internshipTaskLinkRepository.SaveAsync(ct);
            }

            await SendEmailMessageAsync(internshipResult.Data, request.PerformerEmail, ct);

            result.Data.Id = entity.Id;
            result.Data.StatusCode = StatusCodes.Status201Created;
            result.StatusCode = StatusCodes.Status201Created;

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(CreateTasksCommand request, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(request.PerformerEmail))
            {
                Logger.LogError("INTERNSHIP: Possibly unauthorized - no email set from token.");
                throw new ProcessingException("INTERNSHIP: Possibly unauthorized - no email set from token.");
            }

            if (request.InternshipId == Guid.Empty)
            {
                Logger.LogError("INTERNSHIP: Internship Id is not present in the request for creating tasks.");
                throw new ProcessingException("INTERNSHIP: Internship Id is not present in the request for creating tasks.");
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Sends the email notification
        /// </summary>
        /// <param name="internship"></param>
        /// <param name="email"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task SendEmailMessageAsync(Internship internship, string email, CancellationToken ct)
        {
            var emails = new List<string>();

            emails.AddRange(internship.Teachers.Select(x => x.Email).ToList());
            emails.AddRange(internship.Students.Select(x => x.Email).ToList());
            emails.AddRange(internship.CompanyRelatives.Select(x => x.Email).ToList());

            if (!emails.Any()) { return; }

            var authorName = await GetAuthorNameAsync(email, ct);

            var builder = new StringBuilder();
            builder.AppendLine($"{authorName} vytvořil nový úkol u praxe {internship.Name}");
            builder.AppendLine($"Odkaz: {_clientOptions.Url}/internship/edit?id={internship.Id}");

            var notification = new EmailNotification
            {
                Subject = $"Praxe {internship.Name} - nový úkol",
                Message = builder.ToString(),
                Receivers = emails
            };

            await _publisher.Publish(notification, ct);
        }
    }
}
