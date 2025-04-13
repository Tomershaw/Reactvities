using System;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller used to intentionally trigger various types of HTTP errors.
    /// Useful for testing client-side error handling and middleware responses.
    /// </summary>
    public class BuggyController : BaseApiController
    {
        /// <summary>
        /// Returns a 404 Not Found response.
        /// </summary>
        [HttpGet("not-found")]
        public ActionResult GetNotFound()
        {
            return NotFound();
        }

        /// <summary>
        /// Returns a 400 Bad Request response with a custom message.
        /// </summary>
        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest("This is a bad request");
        }

        /// <summary>
        /// Throws a generic server-side exception to simulate a 500 Internal Server Error.
        /// </summary>
        [HttpGet("server-error")]
        public ActionResult GetServerError()
        {
            throw new Exception("This is a server error");
        }

        /// <summary>
        /// Returns a 401 Unauthorized response.
        /// </summary>
        [HttpGet("unauthorised")]
        public ActionResult GetUnauthorised()
        {
            return Unauthorized();
        }
    }
}
