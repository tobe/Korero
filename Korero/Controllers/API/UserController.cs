using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Korero.Repositories;
using Korero.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace Korero.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(
            IThreadRepository threadRepository,
            UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        /// <summary>
        /// Retrieves all threads.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")] // GET /api/user
        public IActionResult GetUser()
        {
            try
            {
                ApplicationUser user = this._userManager.GetUserAsync(HttpContext.User).Result;
                return Ok(user);
            }
            catch (AggregateException)
            {
                return Unauthorized();

            }
        }
    }
}