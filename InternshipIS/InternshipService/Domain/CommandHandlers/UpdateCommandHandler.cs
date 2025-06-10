using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Database.Entities;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Enums;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for updating the internship
    /// </summary>
    public class UpdateCommandHandler : BaseInternshipCommandHandler<UpdateCommand, UpdateResponse>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="internshipRepository"></param>
        /// <param name="internshipTeacherRepository"></param>
        /// <param name="internshipStudentRepository"></param>
        /// <param name="internshipCompanyRelativeRepository"></param>
        /// <param name="internshipCategoryRepository"></param>
        /// <param name="internshipLinkRepository"></param>
        /// <param name="logger"></param>
        public UpdateCommandHandler(
            IInternshipRepository internshipRepository,
            IInternshipTeacherRepository internshipTeacherRepository,
            IInternshipStudentRepository internshipStudentRepository,
            IInternshipCompanyRelativeRepository internshipCompanyRelativeRepository,
            IInternshipCategoryRepository internshipCategoryRepository,
            IInternshipLinkRepository internshipLinkRepository,
            ILogger<UpdateCommandHandler> logger)
            : base(internshipRepository, internshipTeacherRepository, internshipStudentRepository, internshipCompanyRelativeRepository, internshipCategoryRepository, internshipLinkRepository, logger)
        {
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<UpdateResponse>> HandleAsync(UpdateCommand request, CancellationToken ct)
        {
            var internshipResult = await InternshipRepository.GetAsync(request.Id, ct);
            if (internshipResult.HasErrors || internshipResult.Data == null)
            {
                throw new NotFoundException(typeof(Internship).Name, request.Id.ToString());
            }

            var result = new ApiResult<UpdateResponse>();
            var entity = internshipResult.Data;

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.CompanyName = request.CompanyName;
            entity.UpdatedDate = DateTime.Now;
            entity.UpdateAuthor = request.PerformerEmail;

            if (request.Links != null)
            {
                foreach (var link in request.Links)
                {
                    if (link.Id == null)
                    {
                        if (string.IsNullOrEmpty(link.Name)) { continue; }
                        if (string.IsNullOrEmpty(link.Url)) { continue; }

                        var linkEntity = new InternshipLink
                        {
                            Id = Guid.NewGuid(),
                            Name = link.Name,
                            Url = link.Url,
                            CreationAuthor = request.PerformerEmail,
                            CreationDate = DateTime.Now,
                            UpdateAuthor = request.PerformerEmail,
                            UpdatedDate = DateTime.Now,
                            InternshipId = entity.Id
                        };

                        await InternshipLinkRepository.InsertAsync(linkEntity, ct);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(link.Name)) { continue; }
                        if (string.IsNullOrEmpty(link.Url)) { continue; }

                        var linkEntity = InternshipLinkRepository.Get(link.Id.Value).Data;

                        linkEntity.UpdatedDate = DateTime.Now;
                        linkEntity.UpdateAuthor = request.PerformerEmail;
                        linkEntity.Name = link.Name;
                        linkEntity.Url = link.Url;

                        InternshipLinkRepository.Update(linkEntity);
                    }
                }
            }

            var studentId = request.Students != null && request.Students.FirstOrDefault() != null ? request.Students.FirstOrDefault().Id : default;

            var isStudentAdded = await AddStudentsAsync(entity, studentId, ct);
            await AddTeachersAsync(entity, request.Teachers?.Select(x => x.Id)?.ToList(), ct);
            await AddCompanyRelativesAsync(entity, request.CompanyRelatives?.Select(x => x.Id)?.ToList(), ct);
            await AddCategoryAsync(entity, request.CategoryId, ct);

            ResolveState(entity, request.State, isStudentAdded);

            InternshipRepository.Update(entity);
            await InternshipRepository.SaveAsync(ct);

            result.StatusCode = StatusCodes.Status200OK;
            result.Data.StatusCode = StatusCodes.Status200OK;

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(UpdateCommand request, CancellationToken ct)
        {
            if (request.Id == null || request.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Internship Id must be filled.");
            }

            if (string.IsNullOrEmpty(request.PerformerEmail))
            {
                Logger.LogError("INTERNSHIP: Possibly unauthorized - no email set from token.");
                throw new ProcessingException("INTERNSHIP: Possibly unauthorized - no email set from token.");
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                Logger.LogError("INTERNSHIP: Name of internship can not be empty.");
                throw new ProcessingException("INTERNSHIP: Name of the internship can not be empty.");
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Resolve the state of the internship based on the provided state and whether a student was added.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="state"></param>
        /// <param name="isStudentAdded"></param>
        private void ResolveState(Internship entity, int state, bool isStudentAdded)
        {
            entity.State = state;

            if (isStudentAdded)
            {
                entity.SelectedOn = DateTime.Now;
                entity.State = (int)InternshipStateEnum.Chosen;

                return;
            }

            if (entity.State == (int)InternshipStateEnum.Published)
            {
                entity.PublishedOn = DateTime.Now;
            }
            else if (entity.State == (int)InternshipStateEnum.Canceled)
            {
                entity.CanceledOn = DateTime.Now;
            }
            else if (entity.State == (int)InternshipStateEnum.Closed)
            {
                entity.FinishedOn = DateTime.Now;
            }
        }
    }
}
