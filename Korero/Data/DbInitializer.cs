using Korero.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Korero.Models;
using System;
using System.Collections.Generic;

namespace Korero.Data
{
    public class DbInitializer
    {
        private static Random random = new Random();

        private static string RandomString(int Length)
        {
            const string chars = "QWERTYUIOP ";
            return new string(Enumerable.Repeat(chars, Length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static bool RandomBoolean()
        {
            return random.Next(100) < 50;
        }

        public static Thread GenerateRandomThread(ApplicationUser User, ApplicationUser AlternateUser, Tag Tag)
        {
            DateTime Now = DateTime.Now;

            Thread newThread = new Thread()
            {
                Title = RandomString(15),
                DateCreated = Now,
                Author = RandomBoolean() ? User : AlternateUser,
                Tag = Tag,
                Replies = new List<Reply>(),
                Views = random.Next(0, 50)
            };

            for (int i = 0; i < random.Next(4, 10); i++)
            {
                newThread.Replies.Add(new Reply()
                {
                    DateCreated = Now.AddDays(random.Next(-10, -3)).AddHours(random.Next(-24, -1)),
                    DateUpdated = Now.AddHours(random.Next(-23, -2)),
                    Body = RandomString(256),
                    Author = RandomBoolean() ? User : AlternateUser
                });
            }

            return newThread;
        }

        public static async Task InitializeAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            context.Database.EnsureCreated();

            // Look for any users
            if (!context.Users.Any())
            {
                // None found, seed
                var adminUser = new ApplicationUser
                {
                    UserName = "administrator",
                    Email = "localhost@localhost.tld",
                    EmailConfirmed = true
                };

                // Create an admin role btw
                await roleManager.CreateAsync(new IdentityRole("Administrator"));

                // Create the user
                await userManager.CreateAsync(adminUser, "Myp@ss1");

                // Asign them the admin role
                await userManager.AddToRoleAsync(await userManager.FindByNameAsync("administrator"), "Administrator");

                // Create the normal user
                var normalUser = new ApplicationUser
                {
                    UserName = "normaluser",
                    Email = "whatever@whatever.tld",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(normalUser, "Myp@ss2");
            }

            // Look for any threads
            if(!context.Thread.Any())
            {
                // Initialize a couple of threads here
                DateTime Now = DateTime.Now;
                ApplicationUser adminUser = await userManager.FindByNameAsync("administrator");
                ApplicationUser normalUser = await userManager.FindByNameAsync("normaluser");

                // Spawn some random tags
                Tag[] tags =
                {
                    new Tag() { Label = "Casual", Color = "#AAAAAA" },
                    new Tag() { Label = "Important", Color = "#e74c3c" },
                    new Tag() { Label = "Misc", Color = "#3498db" }
                };

                // Add tags into the db
                await context.AddRangeAsync(tags);

                // Spawn 10 random threads
                for(int i = 0; i < 10; i++)
                {
                    Thread t = DbInitializer.GenerateRandomThread(
                        adminUser, normalUser, tags[random.Next(tags.Length)]
                    );

                    await context.Thread.AddAsync(t);
                    await context.Reply.AddRangeAsync(t.Replies);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}