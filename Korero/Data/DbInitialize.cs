using Korero.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;

namespace Korero.Data
{
    public class DbInitialize : IDbInitialize
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitialize(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this._context     = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async void Initialize()
        {
            this._context.Database.EnsureCreated();

            // Try to look for any data --> If true, it's been seeded
            if (this._context.Users.Any()) return;

            var adminUser = new ApplicationUser
            {
                UserName = "localhost@localhost.tld",
                Email = "localhost@localhost.tld",
                EmailConfirmed = true
            };

            // Create an administrator role
            await this._roleManager.CreateAsync(new IdentityRole("Administrator"));

            // Create the admin
            var result = await this._userManager.CreateAsync(adminUser, "Myp@ss1");

            // Add them the "Administrator" role
            await this._userManager.AddToRoleAsync(adminUser, "Administrator");

            await this._context.SaveChangesAsync();
        }
    }
}
