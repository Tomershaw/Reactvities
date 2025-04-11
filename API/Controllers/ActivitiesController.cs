using Microsoft.AspNetCore.Mvc;
using Domain;
using Application.Activities;
using Microsoft.AspNetCore.Authorization;
using Application.Core;
using Application.Profiles;

namespace API.Controllers
{
    // Controller responsible for handling all operations related to activities
    // Inherits shared behavior from BaseApiController (e.g. access to Mediator)

    public class ActivitiesController : BaseApiController
    {
        // GET: api/activities
        // Retrieves a paginated list of activities based on query parameters
        [HttpGet]
        public async Task<IActionResult> GetActivites([FromQuery] ActivityParams param)
        {
            return HandlePagedResult(await Mediator.Send(new List.Query { Params = param }));
        }

        // GET: api/activities/{id}
        // Retrieves details of a specific activity by its unique identifier
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Detalis.Query { Id = id }));
        }

        // POST: api/activities
        // Creates a new activity
        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity)
        {
            return HandleResult(await Mediator.Send(new Create.Commend { Activity = activity }));
        }

        // PUT: api/activities/{id}
        // Updates an existing activity
        // Requires the user to be the host of the activity (authorization policy: IsActivityHost)
        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Commend { Activity = activity }));
        }

        // DELETE: api/activities/{id}
        // Deletes an activity
        // Requires the user to be the host of the activity (authorization policy: IsActivityHost)
        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Commend { Id = id }));
        }

        // POST: api/activities/{id}/attend
        // Toggles the current user's attendance for a specific activity
        [HttpPost("{Id}/attend")]
        public async Task<IActionResult> Attend(Guid id)
        {
            return HandleResult(await Mediator.Send(new UpdateAttendance.Commend { Id = id }));
        }
    }
}
