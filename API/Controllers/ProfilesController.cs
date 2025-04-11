using Application.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Controller responsible for managing user profile-related endpoints
    // Includes fetching profile details and listing user-related activities

    public class ProfilesController : BaseApiController
    {
        // GET: api/profiles/{username}
        // Retrieves public profile information for a given username
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Username = username }));
        }

        // GET: api/profiles/{username}/activities?predicate=...
        // Retrieves activities associated with the user, filtered by predicate
        // Predicate options might include: "past", "future", "hosting", etc.
        [HttpGet("{username}/activities")]
        public async Task<IActionResult> GetUserActivity(string Username, string predicate)
        {
            return HandleResult(await Mediator.Send(new ListActivities.Query { Username = Username, Predicate = predicate }));
        }
    }
}
