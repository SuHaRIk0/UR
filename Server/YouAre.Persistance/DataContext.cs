using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using YouAre.Domain;

namespace YouAre.Persistent
{
    public class DataContext : IdentityDbContext<Account>
	{
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

		public DbSet<User>			Profiles		{  get; set; }
		public DbSet<Community>		Communities		{ get; set; }
		public DbSet<Publication>	Publications	{ get; set; }
		public DbSet<Chat>			Chats			{ get; set; }
		public DbSet<Message>		Messages		{ get; set; }
    }
}
