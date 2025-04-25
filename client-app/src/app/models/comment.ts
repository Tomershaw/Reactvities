// Represents a comment in a chat application
export interface ChatComment {
  id: number; // Unique identifier for the comment
  createAt: Date; // Timestamp when the comment was created
  body: string; // Content of the comment
  username: string; // Username of the comment's author
  displayName: string; // Display name of the comment's author
  image: string; // URL of the author's profile image
}
