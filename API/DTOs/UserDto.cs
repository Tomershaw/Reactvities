// This file contains the UserDto class, which represents a user returned to the client.
// It is used after successful authentication or when fetching user details.

namespace API.DTOs
{
    // Data Transfer Object representing a user returned to the client

    public class UserDto
    {
        // The name displayed in the UI (profile name)
        public string DisplayName { get; set; }

        // The JWT access token used for authenticated requests
        public string Token { get; set; }

        // URL to the user's main profile image
        public string Image { get; set; }

        // The user's unique username
        public string Username { get; set; }

        // Indicates whether the user is allowed to create activities (e.g., host events)
        public bool CanCreateActivity { get; set; }
    }
}
