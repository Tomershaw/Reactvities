using Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain;
using MediatR;
using Application.Activities;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetActivites()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
          return HandleResult(await  Mediator.Send(new Detalis.Query{Id =id}));
        }


        [HttpPost]

        public async Task<IActionResult> CreateActivity(Activity activity) 
        {
            return HandleResult(await Mediator.Send(new Create.Commend {Activity=activity}));
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> EditActivity(Guid id , Activity activity)
        {
            activity.Id = id;
            return HandleResult(await Mediator.Send( new Edit.Commend{Activity =activity}));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Commend{Id =id}));

        }
    }


}