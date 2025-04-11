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

        // Activity category (e.g., music, sports)
        public string Category { get; set; }

        // Date and time of the activity
        public DateTime Date { get; set; }

        // Username of the host (excluded from JSON response)
        [JsonIgnore]
        public string HostUsername { get; set; }
    }
}
