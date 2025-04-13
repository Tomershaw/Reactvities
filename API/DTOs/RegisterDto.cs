// This file contains the RegisterDto class, which is used for user registration requests.
// It includes fields required for creating a new user account.

using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    // Data Transfer Object used for user registration requests

    public class RegisterDto
    {
        // User's display name (e.g., full name shown in the app)
        [Required]
        public string DisplayName { get; set; }

        // User's email address - must be a valid email format
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // User's password - must meet complexity requirements
        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be complex ")]
        public string Password { get; set; }

        // Unique username for login and identification
        [Required]
        public string Username { get; set; }
    }
}
