using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Korero.Repositories;
using Korero.Models;
using Microsoft.AspNetCore.Authorization;

namespace Korero.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IThreadRepository _threadRepository;

        public TagController(
            ITagRepository tagRepository,
            IThreadRepository threadRepository)
        {
            this._tagRepository = tagRepository;
            this._threadRepository = threadRepository;
        }

        [HttpGet] // GET: /api/tag
        public IActionResult GetTags()
        {
            IEnumerable<Tag> tags = this._tagRepository.GetTags();

            if (tags == null || !tags.Any())
                return NotFound();

            return Ok(tags);
        }

        /// <summary>
        /// Adds a single tag
        /// </summary>
        /// <param name="Tag">The tag to add</param>
        /// <returns></returns>
        [HttpPost] // POST: /api/tag
        public IActionResult AddTag([FromBody] Tag Tag)
        {
            // Some validation
            if (Tag == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (this._tagRepository.AddTag(Tag))
                return Ok(Tag);

            return BadRequest();
        }

        /// <summary>
        /// Deletes a single tag
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("{TagId:int}")] // DELETE /api/tag/<TagId:int>
        public IActionResult DeleteTag(int TagId)
        {
            // Check if the said tag exists
            Tag tag = this._tagRepository.GetTag(TagId);
            if (tag == null)
                return BadRequest();

            // Ok, grab all threads which have this tag.
            var threads = this._threadRepository.GetThreadsByTag(tag);

            // Delete all relevant threads and then all relevant tags
            if(this._threadRepository.DeleteMultipleThreads(threads) &&
               this._tagRepository.DeleteTag(tag))
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}