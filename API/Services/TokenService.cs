using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    // Service responsible for creating JWT access tokens and secure refresh tokens
    public class TokenService
    {
        // Provides access to configuration settings (e.g., secret key)
        public IConfiguration _config;

        // Constructor that injects configuration
        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        // Creates a JWT access token for a given user
        public string CreateToken(AppUser user)
        {
            // Define claims to be embedded in the token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };

            // Create a symmetric security key using the secret from configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));

            // Define the signing credentials using HMAC SHA512
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Define token settings including claims, expiration, and signing
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10), // Short-lived access token
                SigningCredentials = creds
            };

            // Create and write the JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Generates a secure refresh token (random 32-byte base64 string)
        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return new RefreshToken { Token = Convert.ToBase64String(randomNumber) };
        }
    }
}
