using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Korero.Data;
using Korero.Models;
using Korero.Repositories;

namespace Korero
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add PostgreSQL
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // Add Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Inject your own settings here...
                // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            });

            // Add application services.
            services.AddScoped<IThreadRepository, ThreadRepository>();

            // Add MVC6
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling =
                                               Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }); // Removes an infinite loop when returning JSON data... 2 good hours wasted.
            /* Note to self: @ manual reverse navigation -> Reply.cs
             * Tldr: Thread references Reply which references Thread which references Reply...
             * (One Thread has multiple replies, and in the Reply model a manual foreign reference
             * to thread has been explicitly made because of another workaround)
             * All this works well with pointers in the actual *memory* of the program, but
             * when returning data as JSON, the return will build up...  infinitely.
             * */

            // Add Database Initializer / Seeder
            services.AddScoped<IDbInitialize, DbInitialize>();

            /*Configure Policies, modern "Roles". Or somewhere between a Role and a Claim.
              Instead of manually generating claims, roles are used via policies to simplify this
              If more control is needed, this can be easily edited to utilize claims:
              options.AddPolicy("RequireElevatedRights", policy => {  
                    policy.RequireClaim("Add User", "Add User");  
                    policy.RequireClaim("Edit User", "Edit User");
                    policy.RequireClaim("Add Thread", "Add Thread");
                    ...
               });
              https://social.technet.microsoft.com/wiki/contents/articles/36959.asp-net-core-mvc-authentication-and-claim-based-authorisation-with-asp-net-identity-core.aspx
             */
            services.AddAuthorization(options =>
            {
                // Administrator only
                options.AddPolicy("RequireAdministrativeRights", policy => policy.RequireRole("Administrator"));
                // Administrators and moderators
                options.AddPolicy("RequireElevatedRights", policy => policy.RequireRole("Administrator", "Moderator"));
                // Everything else -> Users. No need to add a policy for them.
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDbInitialize dbInitialize)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                // TODO: Move the seed here
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Seed
            dbInitialize.Initialize().Wait();
        }
    }
}
