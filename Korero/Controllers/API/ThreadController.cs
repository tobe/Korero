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
        private readonly UserManager<ApplicationUser> _userManager;

        public ThreadController(
            IThreadRepository threadRepository,
            UserManager<ApplicationUser> userManager)
        {
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
        public IActionResult GetThreads(int? page)
        {
            // A page must be non-negative
            if (page <= 0) page = 1;

            var allThreads = this._threadRepository.GetThreads(page);

            // Check if we got anything
            if (allThreads.Item1 == null || !allThreads.Item1.Any())
                return NotFound();

            return Ok(new { total = allThreads.Item2, data = allThreads.Item1 });
        }
    }
}