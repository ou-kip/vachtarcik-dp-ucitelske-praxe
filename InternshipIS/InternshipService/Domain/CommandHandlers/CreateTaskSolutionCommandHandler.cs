using Contracts;
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
    /// The command handler for creatin the solution for task
    /// </summary>
    public class CreateTaskSolutionCommandHandler : BaseCommandHandler<CreateSolutionCommand, CreateSolutionResponse>
    {
        private readonly IInternshipTaskSolutionRepository _repository;
        private readonly IInternshipTaskRepository _taskRepository;
        private readonly IInternshipStudentRepository _studentRepository;
        private readonly IPublishEndpoint _publisher;
        private readonly ClientOptions _clientOptions;

        private InternshipTask Task;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="taskRepository"></param>
        /// <param name="publisher"></param>
        /// <param name="studentRepository"></param>
        /// <param name="clientOptions"></param>
        /// <param name="logger"></param>
        public CreateTaskSolutionCommandHandler(
            IInternshipTaskSolutionRepository repository,
            IInternshipTaskRepository taskRepository,
            IPublishEndpoint publisher,
            IInternshipStudentRepository studentRepository,
            IOptions<ClientOptions> clientOptions,
            ILogger<CreateTaskSolutionCommandHandler> logger) : base(logger)
        {
            _repository = repository;
            _taskRepository = taskRepository;
            _publisher = publisher;
            _clientOptions = clientOptions.Value;
            _studentRepository = studentRepository;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<CreateSolutionResponse>> HandleAsync(CreateSolutionCommand request, CancellationToken ct)
        {
            var result = new ApiResult<CreateSolutionResponse>();

            var studentResult = await _studentRepository.GetByEmailAsync(request.PerformerEmail, ct);
            if (studentResult.HasErrors || studentResult.Data == null)
            {
                throw new NotFoundException(typeof(InternshipStudent).Name, request.PerformerEmail);
            }

            var solution = new InternshipTaskSolution
            {
                Id = Guid.NewGuid(),
                CreationAuthor = request.PerformerEmail,
                CreationDate = DateTime.Now,
                Solution = request.Solution,
                TaskId = Guid.NewGuid(),
                Task = Task,
                UpdateAuthor = request.PerformerEmail,
                UpdatedDate = DateTime.Now,
            };

            Task.State = (int)TaskStateEnum.Submitted;

            await _repository.InsertAsync(solution, ct);
            await _repository.SaveAsync(ct);

            _taskRepository.Update(Task);
            await _taskRepository.SaveAsync(ct);

            await SendEmailMessageAsync(studentResult.Data, ct);

            result.Data.StatusCode = StatusCodes.Status200OK;
            result.StatusCode = StatusCodes.Status200OK;
            result.Data.SolutionId = solution.Id;

            return result;
        }

        ///<inheritdoc/>
        protected override async Task ValidateDataAsync(CreateSolutionCommand request, CancellationToken ct)
        {
            if (request.TaskId == null || request.TaskId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(request.TaskId), "Task ID value must be provided!");
            }

            var taskResult = await _taskRepository.GetAsync(request.TaskId);
            if (taskResult.HasErrors || taskResult.Data == null)
            {
                throw new NotFoundException(nameof(InternshipTask), request.TaskId.ToString());
            }

            Task = taskResult.Data;
        }

        /// <summary>
        /// Sends the email message via notification service
        /// </summary>
        /// <param name="student"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task SendEmailMessageAsync(InternshipStudent student, CancellationToken ct)
        {
            var receiver = Task.CreationAuthor;

            var builder = new StringBuilder();
            builder.AppendLine($"Student: {student.Name} {student.LastName} ({student.StudentCode}) odevzdal/a řešení k úkolu {Task.Name}");
            builder.AppendLine();
            builder.AppendLine($"Odkaz na úkol: {_clientOptions.Url}/task/detail?taskId={Task.Id}");

            var notification = new EmailNotification()
            {
                Receivers = new() { receiver },
                Subject = $"Úkol {Task.Name} - odevzdáno",
                Message = builder.ToString()
            };

            await _publisher.Publish(notification, ct);
        }
    }
}
