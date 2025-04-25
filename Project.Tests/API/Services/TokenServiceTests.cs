using System.Collections.Generic;
using API.Services;
using Domain;
using Microsoft.Extensions.Configuration;
using Xunit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;

namespace Project.Tests.API.Services
{
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                // מפתח עם אורך חוקי (64 תווים ומעלה)
                { "TokenKey", "this_is_a_super_secure_token_key_that_is_definitely_long_enough_1234567890" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _tokenService = new TokenService(configuration);
        }

        [Fact]
        public void CreateToken_Should_Return_Valid_JWT()
        {
            // Arrange
            var user = new AppUser
            {
                Id = "1",
                UserName = "testuser",
                Email = "test@example.com"
            };

            // Act
            var token = _tokenService.CreateToken(user);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(token));

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.NotNull(jwtToken);
            Assert.Contains(jwtToken.Claims, c => c.Type == "unique_name" && c.Value == user.UserName);
            Assert.Contains(jwtToken.Claims, c => c.Type == "nameid" && c.Value == user.Id);
            Assert.Contains(jwtToken.Claims, c => c.Type == "email" && c.Value == user.Email);
        }

        [Fact]
        public void GenerateRefreshToken_Should_Return_Valid_Token()
        {
            // Act
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Assert
            Assert.NotNull(refreshToken);
            Assert.False(string.IsNullOrWhiteSpace(refreshToken.Token));
            Assert.True(refreshToken.Token.Length > 0);
        }

        [Fact]
        public void CreateToken_Should_Throw_Exception_When_TokenKey_Is_Missing()
        {
            // Arrange
            var invalidConfiguration = new ConfigurationBuilder().Build(); // No TokenKey provided
            var tokenService = new TokenService(invalidConfiguration);
            var user = new AppUser
            {
                Id = "1",
                UserName = "testuser",
                Email = "test@example.com"
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => tokenService.CreateToken(user));
        }

        [Fact]
        public void CreateToken_Should_Fail_Validation_When_Token_Is_Expired()
        {
            // Arrange
            var user = new AppUser
            {
                Id = "1",
                UserName = "testuser",
                Email = "test@example.com"
            };

            const string TestTokenKey = "this_is_a_super_secure_token_key_that_is_definitely_long_enough_1234567890";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestTokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var expiredTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.UtcNow.AddMinutes(-20), // Set NotBefore to 20 minutes ago
                Expires = DateTime.UtcNow.AddMinutes(-10),  // Set Expires to 10 minutes ago
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var expiredToken = tokenHandler.CreateToken(expiredTokenDescriptor);
            var expiredTokenString = tokenHandler.WriteToken(expiredToken);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = key,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            // Assert
            Assert.Throws<SecurityTokenExpiredException>(() =>
            {
                tokenHandler.ValidateToken(expiredTokenString, validationParameters, out _);
            });
        }
    }
}
