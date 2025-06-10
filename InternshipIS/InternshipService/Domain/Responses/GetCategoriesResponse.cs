using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    public class GetCategoriesResponse : BaseResponse
    {
        public List<CategoryDto> Categories { get; set; }
    }
}
