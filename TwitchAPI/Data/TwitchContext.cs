using Microsoft.EntityFrameworkCore;
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

            var converter = new EnumCollectionJsonValueConverter<Scope>();
            var comparer = new CollectionValueComparer<Scope>();

            modelBuilder
              .Entity<User>()
              .Property(e => e.Scopes)
              .HasConversion(converter)
              .Metadata.SetValueComparer(comparer);
        }
    }
}
