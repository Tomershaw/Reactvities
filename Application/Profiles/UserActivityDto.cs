// This file contains the UserActivityDto class, which represents a simplified version of an activity.
// It is used to display activity details on a user's profile page.

using System.Text.Json.Serialization;

namespace Application.Profiles
{
    // DTO representing a simplified version of an activity for display on a user's profile page
    public class UserActivityDto
    {
        // Unique identifier of the activity
        public Guid Id { get; set; }

        // Title of the activity
        public string Title { get; set; }

        // Category of the activity (e.g., music, sports, etc.)
        public string Category { get; set; }

        // Date and time when the activity is scheduled to occur
        public DateTime Date { get; set; }

        // Username of the user hosting the activity
        // This property is excluded from JSON responses to avoid exposing sensitive data
        [JsonIgnore]
        public string HostUsername { get; set; }
    }
}
