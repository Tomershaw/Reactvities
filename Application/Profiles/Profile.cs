using Domain;

namespace Application.Profiles
{
    // DTO representing the user's profile information
    // Used for displaying profile details in the client (e.g., profile page)
    public class Profile
    {
        // The unique username of the user (used for routing, lookups, etc.)
        public string UserName { get; set; }

        // The display name of the user shown in the UI
        public string DisplayName { get; set; }

        // A short bio or personal description provided by the user
        public string Bio { get; set; }

        // URL to the user's main profile image
        public string Image { get; set; }

        // Indicates whether the currently logged-in user is following this user
        public bool Following { get; set; }

        // The number of users following this profile
        public int FollowersCount { get; set; }

        // The number of users this profile is following
        public int FollowingCount { get; set; }

        // A collection of photos uploaded by the user
        public ICollection<Photo> Photos { get; set; }
    }
}
