using Contracts;
using Core.Database.Entities;
using Core.Domain.CommandHandlers;
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
    /// The command handler for updating internship tasks
    /// </summary>
    public class UpdateTaskCommandHandler : BaseInternshipCommandHandler<UpdateTaskCommand, UpdateTaskResponse>
    {
        private readonly IInternshipTaskRepository _internshipTaskRepository;
        private readonly IInternshipTaskLinkRepository _internshipTaskLinkRepository;
        private readonly IPublishEndpoint _publisher;
        private readonly ClientOptions _clientOptions;

        public UpdateTaskCommandHandler(
            IInternshipRepository internshipRepository,
            IInternshipTeacherRepository internshipTeacherRepository,
            IInternshipStudentRepository internshipStudentRepository,
            IInternshipCompanyRelativeRepository internshipCompanyRelativeRepository,
            IInternshipCategoryRepository internshipCategoryRepository,
            IInternshipLinkRepository internshipLinkRepository,
            IInternshipTaskRepository internshipTaskRepository,
            IInternshipTaskLinkRepository internshipTaskLinkRepository,
            IPublishEndpoint publisher,
            IOptions<ClientOptions> clientOptions,
            ILogger<UpdateTaskCommandHandler> logger) : base(internshipRepository, internshipTeacherRepository, internshipStudentRepository, internshipCompanyRelativeRepository, internshipCategoryRepository, internshipLinkRepository, logger)
        {
            _internshipTaskLinkRepository = internshipTaskLinkRepository;
            _internshipTaskRepository = internshipTaskRepository;
            _publisher = publisher;
            _clientOptions = clientOptions.Value;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<UpdateTaskResponse>> HandleAsync(UpdateTaskCommand request, CancellationToken ct)
        {
            var result = new ApiResult<UpdateTaskResponse>();

            var taskResult = _internshipTaskRepository.Get(request.Id.Value);
            if (taskResult.HasErrors || taskResult.Data == null)
            {
                throw new NotFoundException(typeof(Internship).Name, request.Id.ToString());
            }

            var task = taskResult.Data;
            task.Name = request.Name;
            task.Description = request.Description;
            task.Summary = request.Summary;
            task.TeacherSummary = request.TeacherSummary;
            task.State = request.State;
            task.IsReported = request.IsReported;
            task.State = task.State;
            task.UpdatedDate = DateTime.Now;
            task.UpdateAuthor = request.PerformerEmail;

            if (request.Links != null && request.Links.Any())
            {
                foreach (var link in request.Links)
                {
                    if (link.Id == null)
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
                            TaskId = task.Id
                        };

                        await _internshipTaskLinkRepository.InsertAsync(linkEntity, ct);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(link.Name)) { continue; }
                        if (string.IsNullOrEmpty(link.Url)) { continue; }

                        var linkEntity = _internshipTaskLinkRepository.Get(link.Id.Value).Data;

                        linkEntity.UpdatedDate = DateTime.Now;
                        linkEntity.UpdateAuthor = request.PerformerEmail;
                        linkEntity.Name = link.Name;
                        linkEntity.Url = link.Url;

                        _internshipTaskLinkRepository.Update(linkEntity);
                    }
                }
            }

            ResolveState(task);

            _internshipTaskRepository.Update(task);
            await _internshipTaskRepository.SaveAsync(ct);

            result.Data.StatusCode = StatusCodes.Status201Created;
            result.StatusCode = StatusCodes.Status201Created;

            await SendEmailMessageAsync(task, request.PerformerEmail, ct);

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(UpdateTaskCommand request, CancellationToken ct)
        {
            if (request.Id == null || request.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Task Id must be filled.");
            }

            if (string.IsNullOrEmpty(request.PerformerEmail))
            {
                Logger.LogError("INTERNSHIP: Possibly unauthorized - no email set from token.");
                throw new ProcessingException("INTERNSHIP: Possibly unauthorized - no email set from token.");
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Sends the email message via notification service
        /// </summary>
        /// <param name="task"></param>
        /// <param name="performerEmail"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task SendEmailMessageAsync(InternshipTask task, string performerEmail, CancellationToken ct)
        {
            var internshipResult = await InternshipRepository.GetAsync(task.InternshipId);
            if (internshipResult.HasErrors || internshipResult.Data == null)
            {
                throw new ProcessingException("Unable to get internship.");
            }

            if (internshipResult.Data.Students == null || !internshipResult.Data.Students.Any())
            {
                return;
            }

            var receivers = internshipResult.Data.Students
                .Select(x => x.Email)
                .ToList();

            var performerName = await GetAuthorNameAsync(performerEmail, ct);

            var builder = new StringBuilder();
            builder.AppendLine($"{performerName} aktualizoval úkol {task.Name} u praxe {internshipResult.Data.Name}");
            builder.AppendLine();
            builder.AppendLine($"Odkaz na úkol: {_clientOptions.Url}/task/detail?taskId={task.Id}");

            var notification = new EmailNotification()
            {
                Receivers = receivers,
                Subject = $"Praxe {internshipResult.Data.Name} - aktualizace úkolu {task.Name} ",
                Message = builder.ToString()
            };

            await _publisher.Publish(notification, ct);
        }

        /// <summary>
        /// Resolves the state of the task based on its current state.
        /// </summary>
        /// <param name="task"></param>
        private void ResolveState(InternshipTask task)
        {
            if (task.State == (int)TaskStateEnum.Done)
            {
                task.IsCompleted = true;
                task.UpdatedDate = DateTime.Now;
            }
            else
            {
                task.IsCompleted = false;
            }
        }
    }
}
