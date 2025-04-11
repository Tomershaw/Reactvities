using Domain;

namespace Application.Profiles
{
    // DTO representing the user's profile information
    // Used for displaying profile details in the client (e.g., profile page)

    public class Profile
    {
        // The unique username (used for routing, lookups, etc.)
        public string UserName { get; set; }

        // The display name shown in the UI
        public string DisplayName { get; set; }

        // User's bio or personal description
        public string Bio { get; set; }

        // URL to the user's main profile image
        public string Image { get; set; }

        // Whether the current logged-in user is following this user
        public bool Following { get; set; }

        // How many users follow this profile
        public int FollowersCount { get; set; }

        // How many users this profile is following
        public int FollowingCount { get; set; }

        // All photos uploaded by this user
        public ICollection<Photo> Photos { get; set; }
    }
}
