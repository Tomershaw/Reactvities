using Application.Photos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller responsible for managing user photos: adding, deleting, and setting the main profile photo.
    /// </summary>
    public class PhotosController : BaseApiController
    {
        /// <summary>
        /// Uploads a new photo for the currently authenticated user.
        /// </summary>
        /// <param name="command">The command containing the photo data.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Add.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        /// <summary>
        /// Deletes the photo with the specified ID from the current user's photo collection.
        /// </summary>
        /// <param name="id">The ID of the photo to delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return HandleResult(await Mediator.Send(new Delete.Commend { Id = id }));
        }

        /// <summary>
        /// Sets the specified photo as the main photo for the user.
        /// </summary>
        /// <param name="id">The ID of the photo to set as main.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(string id)
        {
            return HandleResult(await Mediator.Send(new SetMain.Commend { Id = id }));
        }
    }
}
