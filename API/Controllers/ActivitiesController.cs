using Microsoft.AspNetCore.Mvc;
using Domain;
using Application.Activities;
using Microsoft.AspNetCore.Authorization;
using Application.Core;
using Application.Profiles;

namespace API.Controllers
{
    /// <summary>
    /// Controller responsible for handling all operations related to activities.
    /// Inherits shared behavior from BaseApiController (e.g., access to Mediator).
    /// </summary>
    public class ActivitiesController : BaseApiController
    {
        /// <summary>
        /// Retrieves a paginated list of activities based on query parameters.
        /// </summary>
        /// <param name="param">The query parameters for filtering activities.</param>
        /// <returns>An IActionResult containing the paginated list of activities.</returns>
        [HttpGet]
        public async Task<IActionResult> GetActivites([FromQuery] ActivityParams param)
        {
            return HandlePagedResult(await Mediator.Send(new List.Query { Params = param }));
        }

        /// <summary>
        /// Retrieves details of a specific activity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the activity.</param>
        /// <returns>An IActionResult containing the activity details.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Detalis.Query { Id = id }));
        }

        /// <summary>
        /// Creates a new activity.
        /// </summary>
        /// <param name="activity">The activity to create.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity)
        {
            return HandleResult(await Mediator.Send(new Create.Commend { Activity = activity }));
        }

        /// <summary>
        /// Updates an existing activity.
        /// Requires the user to be the host of the activity.
        /// </summary>
        /// <param name="id">The unique identifier of the activity to update.</param>
        /// <param name="activity">The updated activity data.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Commend { Activity = activity }));
        }

        /// <summary>
        /// Deletes an activity.
        /// Requires the user to be the host of the activity.
        /// </summary>
        /// <param name="id">The unique identifier of the activity to delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Commend { Id = id }));
        }

        /// <summary>
        /// Toggles the current user's attendance for a specific activity.
        /// </summary>
        /// <param name="id">The unique identifier of the activity.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("{Id}/attend")]
        public async Task<IActionResult> Attend(Guid id)
        {
            return HandleResult(await Mediator.Send(new UpdateAttendance.Commend { Id = id }));
        }
    }
}
