// This file contains the ActivityDto class, which represents an activity to be displayed on the client side.
// It includes details such as title, date, location, host username, cancellation status, and attendees.

using Application.Profiles;

namespace Application.Activities
{
    // Data Transfer Object (DTO) representing an activity to be displayed on the client side.
    // This class is used to transfer activity data between the server and client.
    // It includes essential details about the activity, such as its title, date, location, 
    // host information, cancellation status, and attendees.

    public class ActivityDto
    {
        // Unique identifier of the activity, used to distinguish it from other activities.
        public Guid Id { get; set; }

        // Title of the activity, providing a brief description of what the activity is about.
        public string Title { get; set; }

        // Date and time when the activity is scheduled to occur.
        public DateTime Date { get; set; }

        // Detailed description of the activity, explaining its purpose and content.
        public string Description { get; set; }

        // Category of the activity, such as music, sports, or education, for classification purposes.
        public string Category { get; set; }

        // City where the activity will take place, providing geographical context.
        public string City { get; set; }

        // Specific venue or location within the city where the activity will occur.
        public string Venue { get; set; }

        // Username of the user hosting the activity, identifying the organizer.
        public string HostUsername { get; set; }

        // Indicates whether the activity has been cancelled. True if cancelled, otherwise false.
        public bool IsCancelled { get; set; }

        // Collection of attendees participating in the activity, represented as a list of AttendeeDto objects.
        public ICollection<AttendeeDto> Attendees { get; set; }
    }
}
