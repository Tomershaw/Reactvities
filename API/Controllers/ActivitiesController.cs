using Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        private readonly DataContext _context;
        public ActivitiesController(DataContext context)
        {
            _context=context;
        }

         [HttpGet]
           public async Task<ActionResult<List<Activity>>> GetActivites()
           {
            return await _context.Activities.ToListAsync();
           }

            [HttpGet("{id}")]
            public async Task<ActionResult<Activity>> GetActivites(Guid id)
            {
                return await _context.Activities.FindAsync(id);

            }
    }

 
}