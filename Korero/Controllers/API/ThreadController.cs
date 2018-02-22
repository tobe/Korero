using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Korero.Repositories;
using Korero.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Korero.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    public class ThreadController : Controller
    {
        private readonly IThreadRepository _threadRepository;
        private readonly IReplyRepository _replyRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ThreadController(
            IThreadRepository threadRepository,
            IReplyRepository replyRepository,
            UserManager<ApplicationUser> userManager)
        {
            this._replyRepository = replyRepository;
            this._threadRepository = threadRepository;
            this._userManager = userManager;
        }

        /// <summary>
        /// Retrieves all threads.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")] // GET /api/thread
        [Route("page/{page:int?}")] // GET /api/thread/page/[int]
        public IActionResult GetThreads(int? Page)
        {
            // A page must be non-negative
            if (Page <= 0) Page = 1;

            var allThreads = this._threadRepository.GetThreads(Page);

            // Check if we got anything
            if (allThreads.Item1 == null || !allThreads.Item1.Any())
                return NotFound();

            return Ok(new { total = allThreads.Item2, data = allThreads.Item1 });
        }

        /// <summary>
        /// Returns a single thread
        /// </summary>
        /// <param name="ThreadId">The ID of the thread to return</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{ThreadId:int}")] // GET /api/thread/<ThreadId: int>
        public IActionResult GetThread(int ThreadId)
        {
            var thread = this._threadRepository.GetThread(ThreadId);

            if (thread == null)
                return NotFound();

            return Ok(thread);
        }

        /// <summary>
        /// Deletes a thread specified by the ThreadId
        /// </summary>
        /// <param name="ThreadId">The ID of the Thread to delete</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{ThreadId:int}")] // DELETE /api/thread/<id:int>
        public IActionResult DeleteThread(int ThreadId)
        {
            if (this._threadRepository.DeleteThread(ThreadId, User.Identity))
                return Ok();

            return BadRequest();
        }

        /// <summary>
        /// Returns all replies to a thread specified by the id
        /// </summary>
        /// <param name="id">Thread ID</param>
        /// <param name="p">The page to retrieve the results from (pagination)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("replies/{id:int}/page/{p:int}")] // GET /api/thread/r/<id:int>/page/<p:int>
        public IActionResult GetReplies(int id, int p)
        {
            // A page must be non-negative
            if (p <= 0) p = 1;

            var replies = this._replyRepository.GetReplies(id, p);

            if (replies.Item1 == null || !replies.Item1.Any())
                return NotFound();

            return Ok(new { total = replies.Item2, data = replies.Item1 });
        }

        /// <summary>
        /// Adds a new reply. async is used because of the await usermanager...
        /// </summary>
        /// <param name="ThreadId">The ThreadID to add the reply to</param>
        /// <param name="Reply">The reply itself</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{ThreadId:int}")] // POST /api/thread/<ThreadId: int>
        public async Task<IActionResult> AddReply(int ThreadId, [FromBody] Reply Reply)
        {
            // Verify the data model
            if (Reply == null)       return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Grab some stuff we need
            DateTime now = DateTime.Now;
            ApplicationUser user = await this._userManager.GetUserAsync(HttpContext.User);
            Thread thread = this._threadRepository.GetThread(ThreadId);
            if (thread == null) // Verify the thread we're trying to access exists
                return BadRequest();

            // Construct a reply
            Reply newReply = new Reply()
            {
                DateCreated = now,
                DateUpdated = now,
                Author = user,
                Body = Reply.Body,
                Thread = thread
            };

            if (this._replyRepository.AddReply(ThreadId, newReply))
                return Ok(newReply);

            return BadRequest();
        }
    }
}