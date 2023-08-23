using Microsoft.AspNetCore.Mvc;
using Domain;
using Application.Activities;
using Microsoft.AspNetCore.Authorization;
using Application.Core;
using Application.Profiles;

namespace API.Controllers
{
    // [AllowAnonymous]
    public class ActivitiesController : BaseApiController
    {

        [HttpGet]
        public async Task<IActionResult> GetActivites([FromQuery] ActivityParams param)
        {
            return HandlePagedResult(await Mediator.Send(new List.Query{Params = param}));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Detalis.Query { Id = id }));
        }


        [HttpPost]

        public async Task<IActionResult> CreateActivity(Activity activity)
        {
            return HandleResult(await Mediator.Send(new Create.Commend { Activity = activity }));
        }

        [Authorize(Policy ="IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Commend { Activity = activity }));
        }

        [Authorize(Policy ="IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Commend { Id = id }));

        }

        [HttpPost("{Id}/attend")]
        public async Task<IActionResult> Attend (Guid id)
        {
            return HandleResult(await Mediator.Send(new UpdateAttendance.Commend { Id = id }));

        }


    }


}