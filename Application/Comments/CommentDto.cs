namespace Application.Comments
{
    // Data Transfer Object representing a comment.
    // Used to send comment data from the server to the client (e.g., in real-time chat or activity detail view).

    public class CommentDto
    {
        // Unique identifier for the comment
        public int Id { get; set; }

        // Timestamp of when the comment was created
        public DateTime CreateAt { get; set; }

        // The actual content/text of the comment
        public string Body { get; set; }

        // Username of the user who posted the comment
        public string Username { get; set; }

        // Display name of the user who posted the comment
        public string DisplayName { get; set; }

        // URL to the user's profile image
        public string Image { get; set; }
    }
}
