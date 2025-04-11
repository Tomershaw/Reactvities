using System.Security.Cryptography.X509Certificates;
using Application.Followers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Controller responsible for managing follow/unfollow actions and retrieving following/follower lists

    public class FollowController : BaseApiController
    {
        // POST: api/follow/{username}
        // Toggles following/unfollowing the specified user
        // The action is performed by the currently authenticated user
        [HttpPost("{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            return HandleResult(await Mediator.Send(new FollowToggle.Command { TargetUsername = username }));
        }

        // GET: api/follow/{username}?predicate=following|followers
        // Retrieves a list of users that the specified user is following or being followed by
        // Predicate values:
        //    "following" - users that the given username is following
        //    "followers" - users who follow the given username
        [HttpGet("{username}")]
        public async Task<IActionResult> GetFollowings(string username, string predicate)
        {
            return HandleResult(await Mediator.Send(new List.Query { Username = username, Predicate = predicate }));
        }
    }
}
