using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korero.Models;
using Korero.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Korero.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    public class ReplyController : Controller
    {
        private readonly IReplyRepository _replyRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReplyController(
            IReplyRepository replyRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._replyRepository = replyRepository;
            this._userManager = userManager;
        }

        /// <summary>
        /// Updates a single reply
        /// </summary>
        /// <param name="ReplyId">The ID of the reply to update</param>
        /// <param name="NewReply">The new reply</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{ReplyId:int}")] // PUT /api/reply/<ReplyId:int>
        public IActionResult UpdateReply(int ReplyId, [FromBody] Reply NewReply)
        {
            // Validate the model
            if (NewReply == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Grab the current reply
            Reply reply = this._replyRepository.GetReply(ReplyId);
            if (reply == null) // If it doesn't exist, throw 'em a BadRequest
                return BadRequest();

            // Check if the user trying to edit the reply is the currently logged in user
            // Meaning, you can edit only your replies.
            if (reply.Author.UserName != User.Identity.Name)
                return BadRequest();

            // Edit the reply
            reply.DateUpdated = DateTime.Now;
            reply.Body = NewReply.Body;

            // Update!
            if (this._replyRepository.UpdateReply(reply))
                return Ok(reply);

            return BadRequest();
        }

        /// <summary>
        /// Deletes a reply specified by the id
        /// </summary>
        /// <param name="ReplyId">The id of the reply</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{ReplyId:int}")] // DELETE /api/reply/<ReplyId:int>
        public IActionResult DeleteReply(int ReplyId)
        {
            // Grab the reply.
            Reply reply = this._replyRepository.GetReply(ReplyId);
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
    }
}