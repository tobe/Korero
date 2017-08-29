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

        /// <summary>
        /// Returns a single thread
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")] // api/thread/<id:int>
        public IActionResult GetThread(int id)
        {
            var thread = this._threadRepository.GetThread(id);

            // If thread fails, FirstOrDefault will take care of it.
                        
            return Ok(thread);
        }

        /// <summary>
        /// Returns all replies to a thread specified by the id
        /// </summary>
        /// <param name="id">Thread ID</param>
        /// <param name="p">The page to retrieve the results from (pagination)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("r/{id:int}/page/{p:int}")]
        public IActionResult GetReplies(int id, int p)
        {
            // A page must be non-negative
            if (p <= 0) p = 1;

            var replies = this._threadRepository.GetReplies(id, p);

            if (replies == null || !replies.Any())
                return NotFound();

            return Ok(replies);
        }
    }
}