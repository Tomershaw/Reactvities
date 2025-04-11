using Application.Profiles;

namespace Application.Activities
{
    // Data Transfer Object (DTO) representing an activity to be displayed on the client side.
    // Contains key information such as title, date, location, host username, cancellation status,
    // and a list of attendees. This model is typically returned in activity listings and details views.

    public class ActivityDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public string HostUsername { get; set; }
        public bool IsCancelled { get; set; }

        public ICollection<AttendeeDto> Attendees { get; set; }
    }
}
