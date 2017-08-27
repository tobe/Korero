using Korero.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task Initialize()
        {
            this._context.Database.EnsureCreated();

            // Try to look for any data --> If true, it's been seeded
            if (!this._context.Users.Any())
            {
                // Create an admin usser
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
                await this._userManager.AddToRoleAsync(await this._userManager.FindByNameAsync("localhost@localhost.tld"), "Administrator");
            }

            if(!this._context.Thread.Any())
            {
                // Initialize a couple of threads here
                Tag casualTag = new Tag()
                {
                    Label = "Casual",
                    Color = "#AAAAAA"
                };
                Reply genericReply = new Reply()
                {
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Body = "This is a generic reply. How fun."
                };
                Thread[] threads =
                {
                    new Thread() {
                        Title = "Sample Thread",
                        DateCreated = DateTime.Now,
                        Tag = casualTag,
                        Replies = new List<Reply>() {
                            new Reply() { DateCreated = DateTime.Now.AddDays(-1), DateUpdated = DateTime.Now.AddDays(-1), Body = "First sample reply" },
                            new Reply() { DateCreated = DateTime.Now, DateUpdated = DateTime.Now, Body = "Second sample reply" },
                        }
                    },
                    new Thread() {
                        Title = "Lorem ipsum dolor sit amet",
                        DateCreated = DateTime.Now.AddDays(-3),
                        Tag = casualTag,
                        Replies = new List<Reply>() {
                            new Reply() { DateCreated = DateTime.Now.AddDays(-1), DateUpdated = DateTime.Now, Body = "Suspendisse commodo nibh orci, in rutrum tortor faucibus vel. Aenean eu felis vitae leo malesuada aliquet. Mauris ullamcorper urna eu tortor eleifend cursus." },
                            new Reply() { DateCreated = DateTime.Now.AddDays(-5), DateUpdated = DateTime.Now.AddDays(-4), Body = "Some *good* **markdown** right here." },
                        }
                    },
                    new Thread() {
                        Title = "Third time's the charm!",
                        DateCreated = DateTime.Now,
                        Tag = casualTag,
                        Replies = new List<Reply>() {
                            genericReply
                        }
                    }
                };

                // Init a tag, a thread and a reply.
                await this._context.Tag.AddAsync(casualTag);
                foreach (Thread t in threads)
                {
                    await this._context.Thread.AddAsync(t);
                    await this._context.Reply.AddRangeAsync(t.Replies);
                }
            }

            await this._context.SaveChangesAsync();
        }
    }
}
