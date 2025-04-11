using System;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Controller used to intentionally trigger various types of HTTP errors
    // Useful for testing client-side error handling and middleware responses

    public class BuggyController : BaseApiController
    {
        // GET: api/buggy/not-found
        // Returns a 404 Not Found response
        [HttpGet("not-found")]
        public ActionResult GetNotFound()
        {
            return NotFound();
        }

        // GET: api/buggy/bad-request
        // Returns a 400 Bad Request response with a custom message
        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest("This is a bad request");
        }

        // GET: api/buggy/server-error
        // Throws a generic server-side exception to simulate a 500 Internal Server Error
        [HttpGet("server-error")]
        public ActionResult GetServerError()
        {
            throw new Exception("This is a server error");
        }

        // GET: api/buggy/unauthorised
        // Returns a 401 Unauthorized response
        [HttpGet("unauthorised")]
        public ActionResult GetUnauthorised()
        {
            return Unauthorized();
        }
    }
}
