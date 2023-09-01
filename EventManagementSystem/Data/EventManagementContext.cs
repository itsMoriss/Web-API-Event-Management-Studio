using Microsoft.EntityFrameworkCore;
using EventManagementSystem.Models;

namespace EventManagementSystem.Data
{
    public class EventManagementContext : DbContext
    {
        public EventManagementContext(DbContextOptions<EventManagementContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationships here

            // Define the relationship between Event and User
            modelBuilder.Entity<Event>()
                .HasMany(e => e.RegisteredUsers)
                .WithOne(u => u.Event)
                .HasForeignKey(u => u.EventId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete when an event is deleted

            base.OnModelCreating(modelBuilder);
        }
    }
}
