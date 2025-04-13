// This file contains the FacebookDto class and related classes for deserializing Facebook Graph API user data.
// It includes fields such as user ID, email, name, and profile picture.

namespace API.DTOs
{
    // Data Transfer Object for deserializing Facebook Graph API user data
    public class FacebookDto
    {
        // Facebook user ID
        public string Id { get; set; }

        // Facebook user email address
        public string Email { get; set; }

        // Facebook user's full name
        public string Name { get; set; }

        // Facebook profile picture object
        public FacebookPicture Picture { get; set; }
    }

    // Represents the picture field returned by Facebook
    public class FacebookPicture
    {
        // Nested picture data (contains URL, dimensions, etc.)
        public FacebookPictureData Data { get; set; }
    }

    // Contains the actual profile picture URL returned by Facebook
    public class FacebookPictureData
    {
        // URL to the user's Facebook profile picture
        public string Url { get; set; }
    }
}
