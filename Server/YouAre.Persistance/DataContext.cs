using Microsoft.EntityFrameworkCore;

using YouAre.Domain;

namespace YouAre.Persistent
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<User> Profiles { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}