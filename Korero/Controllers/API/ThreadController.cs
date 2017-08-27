using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Korero.Repositories;

namespace Korero.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    public class ThreadController : Controller
    {
        private readonly IThreadRepository _threadRepository;

        public ThreadController(IThreadRepository threadRepository)
        {
            this._threadRepository = threadRepository;
        }

        /// <summary>
        /// Retrieves all threads.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")] // /api/thread
        [Route("p/{p:int?}")] // /api/thread/p/[int]
        public IActionResult GetThreads(int? p)
        {
            // A page must be non-negative
            if (p <= 0) p = 1;

            var allThreads = this._threadRepository.GetThreads(p);

            // Can't make this into a ternary cuz C# is fucking retarded obviously
            if (allThreads.Item1 == null || !allThreads.Item1.Any())
                return NotFound();

            return Ok(new { total = allThreads.Item2, data = allThreads.Item1 });
        }
    }
}