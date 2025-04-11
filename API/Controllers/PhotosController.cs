using Application.Photos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Controller responsible for managing user photos:
    // adding, deleting, and setting the main profile photo

    public class PhotosController : BaseApiController
    {
        // POST: api/photos
        // Uploads a new photo for the currently authenticated user
        // The photo is expected to come from a multipart/form-data form
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Add.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        // DELETE: api/photos/{id}
        // Deletes the photo with the specified ID from the current user's photo collection
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return HandleResult(await Mediator.Send(new Delete.Commend { Id = id }));
        }

        // POST: api/photos/{id}/setMain
        // Sets the specified photo as the main photo for the user
        // Unmarks the current main photo
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(string id)
        {
            return HandleResult(await Mediator.Send(new SetMain.Commend { Id = id }));
        }
    }
}
