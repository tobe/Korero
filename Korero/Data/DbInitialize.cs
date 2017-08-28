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
                    UserName = "administrator",
                    Email = "localhost@localhost.tld",
                    EmailConfirmed = true
                };
                // Create an administrator role
                await this._roleManager.CreateAsync(new IdentityRole("Administrator"));
                // Create the admin
                var result = await this._userManager.CreateAsync(adminUser, "Myp@ss1");
                // Add them the "Administrator" role
                await this._userManager.AddToRoleAsync(await this._userManager.FindByNameAsync("administrator"), "Administrator");
            }

            if(!this._context.Thread.Any())
            {
                // Initialize a couple of threads here
                DateTime Now = DateTime.Now;
                ApplicationUser adminUser = await this._userManager.FindByNameAsync("administrator");

                Tag casualTag = new Tag()
                {
                    Label = "Casual",
                    Color = "#AAAAAA"
                };
                Reply genericReply = new Reply()
                {
                    DateCreated = Now,
                    DateUpdated = Now,
                    Body = "This is a generic reply. How fun.",
                    Author = adminUser
                };
                Thread[] threads =
                {
                    new Thread() {
                        Title = "Sample Thread",
                        DateCreated = Now,
                        Tag = casualTag,
                        Replies = new List<Reply>() {
                            new Reply() { DateCreated = Now.AddDays(-1), DateUpdated = Now.AddDays(-1), Body = "First sample reply", Author = adminUser},
                            new Reply() { DateCreated = Now, DateUpdated = Now, Body = "Second sample reply", Author = adminUser},
                        },
                        Author = adminUser
                    },
                    new Thread() {
                        Title = "Lorem ipsum dolor sit amet",
                        DateCreated = Now.AddDays(-3),
                        Tag = casualTag,
                        Replies = new List<Reply>() {
                            new Reply() { DateCreated = Now.AddDays(-1), DateUpdated = Now, Body = "Suspendisse commodo nibh orci, in rutrum tortor faucibus vel. Aenean eu felis vitae leo malesuada aliquet. Mauris ullamcorper urna eu tortor eleifend cursus.", Author = adminUser},
                            new Reply() { DateCreated = Now.AddDays(-5), DateUpdated = Now.AddDays(-4), Body = "Some *good* **markdown** right here.", Author = adminUser},
                        },
                        Author = adminUser
                    },
                    new Thread() {
                        Title = "Third time's the charm!",
                        DateCreated = Now,
                        Tag = casualTag,
                        Replies = new List<Reply>() {
                            genericReply
                        },
                        Author = adminUser
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
