using Microsoft.EntityFrameworkCore;
using MMS.API.Models;

namespace MMS.API.Data
{
    public class MMSDbContext : DbContext
    {
        public MMSDbContext(DbContextOptions<MMSDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Meeting> Meetings { get; set; }

        public DbSet<Meeting> Permissions_Role { get; set; }

    }
}
