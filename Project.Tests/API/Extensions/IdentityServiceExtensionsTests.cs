// using API;
// using API.Services;
// using Domain;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.Extensions.DependencyInjection;
// using System;
// using Xunit;
// using Microsoft.AspNetCore.Mvc.Testing;

// namespace Project.Tests.API.Extensions
// {
//     public class IdentityServiceExtensionsTests : IClassFixture<WebApplicationFactory<Program>>
//     {
//         private readonly WebApplicationFactory<Program> _factory;

//         public IdentityServiceExtensionsTests(WebApplicationFactory<Program> factory)
//         {
//             _factory = factory;
//         }

//         [Fact]
//         public void Should_Resolve_Identity_Services_From_DI()
//         {
//             // Arrange
//             var scope = _factory.Services.CreateScope();
//             var services = scope.ServiceProvider;

//             // Act
//             var userManager = services.GetService<UserManager<AppUser>>();
//             var signInManager = services.GetService<SignInManager<AppUser>>();
//             var tokenService = services.GetService<TokenService>();

//             // Assert
//             Assert.NotNull(userManager);
//             Assert.NotNull(signInManager);
//             Assert.NotNull(tokenService);
//         }
//     }
// }
