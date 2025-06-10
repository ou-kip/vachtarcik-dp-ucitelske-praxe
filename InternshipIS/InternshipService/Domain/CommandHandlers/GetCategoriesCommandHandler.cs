using Core.Domain.CommandHandlers;
using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for getting the collection of internship categories
    /// </summary>
    public class GetCategoriesCommandHandler : BaseCommandHandler<GetCategoriesCommand, GetCategoriesResponse>
    {
        private readonly IInternshipCategoryRepository _internshipCategoryRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="internshipCategoryRepository"></param>
        /// <param name="logger"></param>
        public GetCategoriesCommandHandler(
            IInternshipCategoryRepository internshipCategoryRepository,
            ILogger<GetCategoriesCommandHandler> logger) : base(logger)
        {
            _internshipCategoryRepository = internshipCategoryRepository;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<GetCategoriesResponse>> HandleAsync(GetCategoriesCommand request, CancellationToken ct)
        {
            var result = new ApiResult<GetCategoriesResponse>();

            var categoriesResult = await _internshipCategoryRepository.GetAllAsync(ct: ct);
            if (categoriesResult.HasErrors)
            {
                Logger.LogError(categoriesResult.GetErrorMessage());
                throw new ProcessingException(categoriesResult.GetErrorMessage());
            }

            result.Data.Categories = categoriesResult.Data.Select(x =>  new CategoryDto(x.Id, x.Code, x.Name)).ToList();

            result.StatusCode = StatusCodes.Status200OK;
            result.Data.StatusCode = StatusCodes.Status200OK;

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(GetCategoriesCommand request, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}
