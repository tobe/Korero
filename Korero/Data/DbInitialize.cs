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

        private static Random random = new Random();

        public DbInitialize(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this._context     = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private Thread RandomThread(ApplicationUser user, Tag tag)
        {
            DateTime Now = DateTime.Now;
            Thread newThread = new Thread()
            {
                Title = RandomString(15),
                DateCreated = DateTime.Now,
                Views = random.Next(0, 50),
                Tag = tag,
                Replies = new List<Reply>(),
                Author = user
            };

            for(int i = 0; i < random.Next(1, 10); i++)
            {
                newThread.Replies.Add(new Reply
                {
                    DateCreated = Now.AddDays(-1),
                    DateUpdated = Now.AddDays(-1),
                    Body = RandomString(256),
                    Author = user
                });
            }

            return newThread;
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
                // Init a tag, a thread and a reply.
                await this._context.Tag.AddAsync(casualTag);
                for(int i = 0; i < 10; i++)
                {
                    Thread t = this.RandomThread(adminUser, casualTag);
                    await this._context.Thread.AddAsync(t);
                    await this._context.Reply.AddRangeAsync(t.Replies);
                }
            }

            await this._context.SaveChangesAsync();
        }
    }
}
