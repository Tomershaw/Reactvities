using System.Text;
using API.Services;
using Domain;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions
{
    /// <summary>
    /// Configures identity, authentication, and authorization services for the application.
    /// </summary>
    public static class IdentityServiceExtensions
    {
        /// <summary>
        /// Adds identity, authentication, and authorization services to the application.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="config">The application configuration object.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            // Configure ASP.NET Identity for user management with custom options
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false; // Simplify password requirements
                opt.User.RequireUniqueEmail = true;          // Ensure email uniqueness
            })
            .AddEntityFrameworkStores<DataContext>();

            services.AddAuthentication();

            // Create a symmetric security key from the secret in app settings
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

            // Register the token generation service
            services.AddScoped<TokenService>();

            // Configure JWT authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero // Disable time tolerance for expired tokens
                    };

                    // Allow JWT tokens via query string for SignalR (e.g., /chat?access_token=...)
                    opt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            // Configure authorization policy: "IsActivityHost"
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("IsActivityHost", policy =>
                {
                    policy.Requirements.Add(new IsHostRequirement());
                });
            });

            // Register handler for the "IsActivityHost" policy
            services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();

            return services;
        }
    }
}

// Summary: This file provides extension methods for configuring identity, authentication, and authorization services.
