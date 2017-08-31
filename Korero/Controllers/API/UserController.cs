using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Korero.Models;

namespace Korero.Controllers.API
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        [HttpGet] // GET: /api/user
        public IActionResult GetUserInformation()
        {
            try
            {
                ApplicationUser user = this._userManager.GetUserAsync(HttpContext.User).Result;
                return Ok(user);
            }catch(AggregateException)
            {
                return Unauthorized();
            }
        }
    }
}
