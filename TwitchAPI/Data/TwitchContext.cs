using Microsoft.EntityFrameworkCore;
using System;
using TwitchAPI.Models;

namespace TwitchAPI.Data
{
    public class TwitchContext : DbContext
    {
        public TwitchContext(DbContextOptions<TwitchContext> opt) : base(opt)
        {
        }

        public DbSet<App> Apps { get; set; }

        public DbSet<User> Users { get; set; }

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
