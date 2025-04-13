using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    /// <summary>
    /// Service responsible for creating JWT access tokens and secure refresh tokens.
    /// </summary>
    public class TokenService
    {
        /// <summary>
        /// Provides access to configuration settings (e.g., secret key).
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// Constructor that injects configuration.
        /// </summary>
        /// <param name="config">The application configuration.</param>
        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Creates a JWT access token for a given user.
        /// </summary>
        /// <param name="user">The user for whom the token is created.</param>
        /// <returns>A signed JWT access token as a string.</returns>
        public string CreateToken(AppUser user)
        {
            // Define claims to be embedded in the token, representing user identity and attributes.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName), // User's username
                new Claim(ClaimTypes.NameIdentifier, user.Id), // User's unique identifier
                new Claim(ClaimTypes.Email, user.Email), // User's email address
            };

            // Create a symmetric security key using the secret key from configuration.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));

            // Define the signing credentials using HMAC SHA512 algorithm.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Define token settings including claims, expiration time, and signing credentials.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // Attach claims to the token
                Expires = DateTime.UtcNow.AddMinutes(10), // Set token expiration time
                SigningCredentials = creds // Use the defined signing credentials
            };

            // Create and write the JWT token using the token handler.
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); // Return the serialized token
        }

        /// <summary>
        /// Generates a secure refresh token (random 32-byte base64 string).
        /// </summary>
        /// <returns>A new refresh token object.</returns>
        public RefreshToken GenerateRefreshToken()
        {
            // Generate a random 32-byte array for the refresh token.
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            // Return the refresh token as a base64-encoded string.
            return new RefreshToken { Token = Convert.ToBase64String(randomNumber) };
        }
    }
}
