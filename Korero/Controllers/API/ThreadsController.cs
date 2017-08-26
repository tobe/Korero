using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Korero.Repositories;

namespace Korero.Controllers.API
{
    [Authorize]
    //[Route("api/[controller]")]
    public class ThreadsController : Controller
    {
        private readonly IThreadRepository _threadRepository;

        public ThreadsController(IThreadRepository threadRepository)
        {
            this._threadRepository = threadRepository;
        }

        [HttpGet("api/threads")]
        public IActionResult GetThreads()
        {
            // TODO: try/catch
            var allThreads = this._threadRepository.GetThreads();

            // Can't make this into a ternary cuz C# is fucking retarded obviously
            if (allThreads == null || !allThreads.Any())
                return NotFound();

            return Ok(allThreads);
        }
    }
}