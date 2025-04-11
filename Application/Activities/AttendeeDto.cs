using Domain;

namespace Application.Activities
{
    // Data Transfer Object representing a user attending an activity.
    // Used to display attendee information in activity listings or details view.
    // Includes profile info and social data (following/followers count).

    public class AttendeeDto
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public bool Following { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
    }
}
