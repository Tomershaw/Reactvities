using Application.Activities;
using Application.Core;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Photos;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions
{
    /// <summary>
    /// Registers application-level services such as database context, CORS, MediatR, and others.
    /// </summary>
    public static class ApplicationServiceExtensions
    {
        /// <summary>
        /// Configures application-level services.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="config">The application configuration object.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Swagger/OpenAPI support
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Configures the database context with environment-based connection string
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            // Configures CORS to allow client app communication (e.g. React frontend)
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("www-Authenticate", "pagination")
                        .WithOrigins("http://localhost:3000", "https://localhost:3000");
                });
            });

            // MediatR registration for CQRS handlers
            services.AddMediatR(typeof(List.Handler));

            // AutoMapper registration for DTO <-> Domain mapping
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);

            // FluentValidation registration for automatic model validation
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Create>();

            // HttpContext accessor for accessing current user info
            services.AddHttpContextAccessor();

            // Dependency injection for custom interfaces/services
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();

            // Configuration for Cloudinary image storage service
            services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));

            // SignalR registration for real-time communication (e.g. chat)
            services.AddSignalR();

            return services;
        }
    }
}
