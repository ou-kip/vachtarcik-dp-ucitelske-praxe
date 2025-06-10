using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    public class AllowedActionsResponse : BaseResponse
    {
        public AllowedActionsDto AllowedActions { get; set; }
    }
}
