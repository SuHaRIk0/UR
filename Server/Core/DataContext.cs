using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Server.Core
{
	public class DataContext : IdentityDbContext<Account>
	{
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

		public DbSet<Community> Communities { get; set; }
    }
}
