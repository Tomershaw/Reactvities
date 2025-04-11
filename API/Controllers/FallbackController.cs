using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Controller that serves the fallback route for unmatched client-side routes
    // Used to support single-page applications (SPA) with client-side routing

    [AllowAnonymous]
    public class FallbackController : Controller
    {
        // GET: fallback route (e.g., when no other API or static file matches the request)
        // Returns the SPA entry point: index.html from the wwwroot directory
        public IActionResult Index()
        {
            return PhysicalFile(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"),
                "text/HTML"
            );
        }
    }
}
// This controller is typically used in conjunction with a front-end framework (e.g., React, Angular)
// that handles routing on the client side. When a user navigates to a route that doesn't exist on the server,
// the request is directed to this controller, which serves the main HTML file for the SPA.
// This allows the front-end application to take over and render the appropriate view based on its own routing logic.
// The controller is marked with [AllowAnonymous] to ensure that it can be accessed without authentication,