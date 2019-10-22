using System.Data.Entity;

namespace MainApp.Models
{
    public class ConnectionContext : DbContext
    {
        public ConnectionContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
    }
}