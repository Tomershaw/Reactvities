// This file contains the CommentDto class, which represents a comment.
// It is used to send comment data from the server to the client.

namespace Application.Comments
{
    // Data Transfer Object (DTO) representing a comment.
    // This is used to transfer comment data between the server and client.

    public class CommentDto
    {
        // Unique identifier for the comment.
        // This is the primary key for identifying a comment.
        public int Id { get; set; }

        // Timestamp indicating when the comment was created.
        // This helps in sorting or displaying comments chronologically.
        public DateTime CreateAt { get; set; }

        // The actual content or text of the comment.
        // This is the main body of the comment.
        public string Body { get; set; }

        // The username of the user who posted the comment.
        // This is used to identify the author of the comment.
        public string Username { get; set; }

        // The display name of the user who posted the comment.
        // This is a more user-friendly name for display purposes.
        public string DisplayName { get; set; }

        // URL to the profile image of the user who posted the comment.
        // This is used to display the user's avatar alongside the comment.
        public string Image { get; set; }
    }
}
