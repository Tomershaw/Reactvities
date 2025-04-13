using System.Security.Cryptography.X509Certificates;
using Application.Followers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller responsible for managing follow/unfollow actions and retrieving following/follower lists.
    /// </summary>
    public class FollowController : BaseApiController
    {
        /// <summary>
        /// Toggles following/unfollowing the specified user.
        /// </summary>
        /// <param name="username">The username of the target user.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            return HandleResult(await Mediator.Send(new FollowToggle.Command { TargetUsername = username }));
        }

        /// <summary>
        /// Retrieves a list of users that the specified user is following or being followed by.
        /// </summary>
        /// <param name="username">The username of the target user.</param>
        /// <param name="predicate">The predicate to filter the list ("following" or "followers").</param>
        /// <returns>An IActionResult containing the list of users.</returns>
        [HttpGet("{username}")]
        public async Task<IActionResult> GetFollowings(string username, string predicate)
        {
            return HandleResult(await Mediator.Send(new List.Query { Username = username, Predicate = predicate }));
        }
    }
}
