using Korero.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                await this._userManager.AddToRoleAsync(adminUser, "Administrator");
            }

            if(!this._context.Thread.Any())
            {
                // Init a tag, a thread and a reply.
                Thread sampleThread = new Thread()
                {
                    Title = "Sample Thread",
                    DateCreated = DateTime.Now,
                    Tag = new Tag()
                    {
                        Label = "Casual",
                        Color = "#AAAAAA"
                    },
                    Replies = new List<Reply>()
                {
                    new Reply() { DateCreated = DateTime.Now.AddDays(-1), DateUpdated = DateTime.Now.AddDays(-1), Body = "First sample reply" },
                    new Reply() { DateCreated = DateTime.Now, DateUpdated = DateTime.Now, Body = "Second sample reply" },
                }
                };

                await this._context.Thread.AddAsync(sampleThread);
                await this._context.Tag.AddAsync(sampleThread.Tag);
                await this._context.Reply.AddRangeAsync(sampleThread.Replies);
            }

            await this._context.SaveChangesAsync();
        }
    }
}
