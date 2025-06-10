namespace InternshipService.Domain
{
    public class CalendarEventDto
    {
        public CalendarEventDto() { }

        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public string EventTime { get; set; }
        public int EventType { get; set; }
    }
}
