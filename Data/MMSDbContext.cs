using Microsoft.EntityFrameworkCore;
using MMS.API.Models;

namespace MMS.API.Data
{
    public class MMSDbContext : DbContext
    {
        // Constructor accepting DbContextOptions
        public MMSDbContext(DbContextOptions<MMSDbContext> options) : base(options)
        {
        }

        // DbSet definitions
        public required DbSet<User> Users { get; set; }
        public required DbSet<Role> Roles { get; set; }
        public required DbSet<Permissions> Permissions { get; set; }
        public required DbSet<Meeting> Meetings { get; set; }
        public required DbSet<Permission_Role> Permission_Role { get; set; }
        public required DbSet<Attendee> Attendees { get; set; }
        public required DbSet<MeetingMinute> Minutes { get; set; } // Add this line

        // Configure relationships and mappings
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Permission_Role relationships
            modelBuilder.Entity<Permission_Role>()
                .HasKey(pr => pr.Id); // Primary key

            modelBuilder.Entity<Permission_Role>()
                .HasOne(pr => pr.Role)
                .WithMany()
                .HasForeignKey(pr => pr.RoleId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete on Role deletion

            modelBuilder.Entity<Permission_Role>()
                .HasOne(pr => pr.Permission)
                .WithMany()
                .HasForeignKey(pr => pr.PermissionId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete on Permission deletion
            
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);



            // Configure Attendee relationships
            modelBuilder.Entity<Attendee>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Attendee>()
                .HasOne(a => a.Meeting)
                .WithMany(m => m.Attendees)
                .HasForeignKey(a => a.MeetingId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: cascade delete attendees when a meeting is deleted

            modelBuilder.Entity<Attendee>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete of users

            // Configure Meeting relationships
            modelBuilder.Entity<Meeting>()
                .HasKey(m => m.MeetingId);

            modelBuilder.Entity<Meeting>()
                .HasOne(m => m.Organizer)
                .WithMany()
                .HasForeignKey(m => m.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of organizer if referenced

            // Add other entity configurations as needed
        }
    }
}
