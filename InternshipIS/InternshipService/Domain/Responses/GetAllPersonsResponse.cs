using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    public class GetAllPersonsResponse : BaseResponse
    {
        public List<PersonDto> Accounts { get; set; }
    }
}
