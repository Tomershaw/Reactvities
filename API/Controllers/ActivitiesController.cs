using Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain;
using MediatR;
using Application.Activities;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetActivites()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivites(Guid id)
        {
            return await  Mediator.Send(new Detalis.Query{Id =id});
        }


        [HttpPost]

        public async Task<IActionResult> CreateActivity(Activity activity) 
        {
            return Ok(await Mediator.Send(new Create.Commend {Activity=activity}));
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> EditActivity(Guid id , Activity activity)
        {
            activity.Id = id;
            return Ok(await Mediator.Send( new Edit.Commend{Activity =activity}));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteActivity(Guid id)
        {
            return Ok(await Mediator.Send(new Delete.Commend{Id =id}));

        }
    }


}