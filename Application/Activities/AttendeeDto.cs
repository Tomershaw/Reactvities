// This file contains the AttendeeDto class, which represents a user attending an activity.
// It includes profile information and social data such as followers and following counts.

namespace Application.Activities
{
    // Data Transfer Object representing a user attending an activity.
    // This class is used to transfer attendee data between the server and client.
    // It includes profile information and social data such as followers and following counts.

    public class AttendeeDto
    {
        // Username of the attendee, uniquely identifying the user.
        public string UserName { get; set; }

        // Display name of the attendee, shown in the UI.
        public string DisplayName { get; set; }

        // Short biography of the attendee, providing additional information about the user.
        public string Bio { get; set; }

        // URL to the attendee's profile image, used for displaying their avatar.
        public string Image { get; set; }

        // Indicates if the current user is following this attendee. True if following, otherwise false.
        public bool Following { get; set; }

        // Number of followers the attendee has, representing their social reach.
        public int FollowersCount { get; set; }

        // Number of users the attendee is following, representing their social connections.
        public int FollowingCount { get; set; }
    }
}
