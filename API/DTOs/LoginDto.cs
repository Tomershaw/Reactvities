namespace API.DTOs
{
    // Data Transfer Object used for user login requests
    // Contains user-provided credentials for authentication

    public class LoginDto
    {
        // User's email address used for login
        public string Email { get; set; }

        // User's password (plain text, to be validated against the hashed version)
        public string Password { get; set; }
    }
}
