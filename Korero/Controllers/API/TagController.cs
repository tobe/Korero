using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Korero.Repositories;
using Korero.Models;

namespace Korero.Controllers.API
{
    [Route("api/[controller]")]
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;

        public TagController(ITagRepository tagRepository)
        {
            this._tagRepository = tagRepository;
        }

        [HttpGet] // GET: /api/tag
        public IActionResult GetTags()
        {
            IEnumerable<Tag> tags = this._tagRepository.GetTags();

            if (tags == null || !tags.Any())
                return NotFound();

            return Ok(tags);
        }
    }
}
