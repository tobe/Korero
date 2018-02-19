using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Korero.Models;

namespace Korero.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Korero.Models.ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Korero.Models.Tag> Tag { get; set; }
        public DbSet<Korero.Models.Reply> Reply { get; set; }
        public DbSet<Korero.Models.Thread> Thread { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
