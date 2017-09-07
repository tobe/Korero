using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Korero.Repositories;
using Korero.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Korero.Models.ApiDtos;

namespace Korero.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    public class ThreadController : Controller
    {
        private readonly IThreadRepository _threadRepository;
        private readonly IReplyRepository _replyRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITagRepository _tagRepository;

        public ThreadController(
            IThreadRepository threadRepository,
            IReplyRepository replyRepository,
            ITagRepository tagRepository,
            UserManager<ApplicationUser> userManager)
        {
            this._threadRepository = threadRepository;
            this._userManager = userManager;
            this._replyRepository = replyRepository;
            this._tagRepository = tagRepository;
        }

        /// <summary>
        /// Retrieves all threads.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")] // GET /api/thread
        [Route("p/{p:int?}")] // GET /api/thread/p/[int]
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
        [Route("{id:int}")] // GET /api/thread/<id:int>
        public IActionResult GetThread(int id)
        {
            var thread = this._threadRepository.GetThread(id);

            if (thread == null)
                return NotFound();

            return Ok(thread);
        }

        /// <summary>
        /// Creates a new thread
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")] //  POST /api/thread
        public async Task<IActionResult> CreateThread([FromBody] NewThreadDto data)
        {
            if (data == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Check whether the tag even exists
            Tag tag = this._tagRepository.GetTag(data.TagId);
            if (tag == null)
                return BadRequest();

            // Tag exists, the title is verified by the first two lines.
            // Construct a Thread and add it.
            ApplicationUser user = await this._userManager.GetUserAsync(HttpContext.User);
            Thread thread = new Thread()
            {
                Title = data.Title,
                DateCreated = DateTime.Now,
                Views = 0,
                Author = user,
                Tag = tag
            };

            if (this._threadRepository.AddThread(thread))
                return Ok(thread);

            return BadRequest();
        }

        [HttpPost]
        [Route("{id:int}")] // POST /api/thread/<id:int>
        // Task<IActionResult> because of the await userManager
        public async Task<IActionResult> AddReply(int id, [FromBody] Reply data) {
            if (data == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState); // Type validation, p much

            // Grab some stuff we need
            DateTime now = DateTime.Now;
            ApplicationUser user = await this._userManager.GetUserAsync(HttpContext.User);
            Thread thread = this._threadRepository.GetThread(id);
            if (thread == null) // Verify the thread we're trying to access exists
                return BadRequest();

            // Construct a reply
            Reply reply = new Reply()
            {
                DateCreated = now,
                DateUpdated = now,
                Author = user,
                Body = data.Body,
                Thread = thread
            };

            if(this._replyRepository.AddReply(id, reply))
                return Ok(reply);

            return BadRequest();
        }

        /// <summary>
        /// Deletes a reply specified by the id
        /// </summary>
        /// <param name="id">The id of the reply</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("r/{id:int}")]
        public IActionResult DeleteReply(int id)
        {
            // Grab the reply.
            Reply reply = this._replyRepository.GetReply(id);
            if (reply == null)
                return BadRequest();

            // Check whether the user is trying to delete their own reply
            if (reply.Author.UserName != User.Identity.Name)
                return BadRequest();

            // So far so good, delete it
            if (this._replyRepository.DeleteReply(reply))
                return NoContent();

            return BadRequest();
        }

        [HttpPut]
        [Route("r/{id:int}")] // PUT /api/thread/r/<id:int>
        public IActionResult UpdateReply(int id, [FromBody] Reply data)
        {
            // Validate the model
            if (data == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Grab the current reply
            Reply reply = this._replyRepository.GetReply(id);
            if (reply == null) // If it doesn't exist, throw 'em a BadRequest
                return BadRequest();

            // Check if the user trying to edit the reply is the currently logged in user
            // Meaning, you can edit only your replies.
            if (reply.Author.UserName != User.Identity.Name)
                return BadRequest();

            // Edit the reply
            reply.DateUpdated = DateTime.Now;
            reply.Body = data.Body;

            // Update!
            if (this._replyRepository.UpdateReply(reply))
                return Ok(reply);

            return BadRequest();
        }

        [HttpDelete]
        [Route("{id:int}")] // DELETE /api/thread/<id:int>
        public IActionResult DeleteThread(int id)
        {
            if (this._threadRepository.DeleteThread(id, User.Identity))
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
        [Route("r/{id:int}/page/{p:int}")] // GET /api/thread/r/<id:int>/page/<p:int>
        public IActionResult GetReplies(int id, int p)
        {
            // A page must be non-negative
            if (p <= 0) p = 1;

            var replies = this._replyRepository.GetReplies(id, p);

            if (replies.Item1 == null || !replies.Item1.Any())
                return NotFound();

            return Ok(new { total = replies.Item2, data = replies.Item1 });
        }
    }
}