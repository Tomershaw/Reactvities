// This file contains the LoginDto class, which is used for user login requests.
// It includes fields for user-provided credentials.

namespace API.DTOs
{
    // Data Transfer Object used for user login requests

    public class LoginDto
    {
        // User's email address used for login
        public string Email { get; set; }

        // User's password (plain text, to be validated against the hashed version)
        public string Password { get; set; }
    }
}
