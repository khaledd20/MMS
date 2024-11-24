using Microsoft.EntityFrameworkCore;
using MMS.API.Models;

namespace MMS.API.Data
{
    public class MMSDbContext(DbContextOptions<MMSDbContext> options) : DbContext(options)
    {
        public required DbSet<User> Users { get; set; }
        
        public required DbSet<Role> Roles { get; set; }
        public required DbSet<Permissions> Permissions { get; set; }
        public required DbSet<Meeting> Meetings { get; set; }

        public required DbSet<Permission_Role> Permission_Role { get; set; }

        public required DbSet<Attendee> Attendees { get; set; }

    }
}
