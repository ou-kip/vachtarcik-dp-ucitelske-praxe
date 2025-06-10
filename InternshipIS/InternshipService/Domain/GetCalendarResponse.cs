using Core.Domain.Responses;

namespace InternshipService.Domain
{
    public class GetCalendarResponse : BaseResponse
    {
        public List<CalendarEventDto> Events { get; set; }
    }
}
