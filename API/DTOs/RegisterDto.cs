using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    // Data Transfer Object used for user registration requests
    // Contains required fields for creating a new user account

    public class RegisterDto
    {
        // User's display name (e.g. full name shown in the app)
        [Required]
        public string DisplayName { get; set; }

        // User's email address - must be a valid email format
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // User's password - must be between 4 and 8 characters, contain at least one digit,
        // one lowercase letter, and one uppercase letter
        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be complex ")]
        public string Password { get; set; }

        // Unique username for login and identification
        [Required]
        public string Username { get; set; }
    }
}
