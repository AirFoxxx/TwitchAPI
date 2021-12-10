using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using TwitchAPI.Models;
using TwitchAPI.Models.AppUsers;

namespace TwitchAPI.Data
{
    public class TwitchContext : IdentityDbContext<ApplicationUser>
    {
        public TwitchContext(DbContextOptions<TwitchContext> opt) : base(opt)
        {
        }

        public DbSet<App> Apps { get; set; }

        public DbSet<User> TwitchUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                        .Property(e => e.Scopes)
                        .HasConversion(
                            v => string.Join(',', v),
                            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
