using basicbanking.api.Domain;
using Microsoft.EntityFrameworkCore;

namespace basicbanking.api.Data
{
	public class PostgresDbContext : DbContext
	{
		public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasOne<BankAccount>(s => s.Account)
				.WithOne(cs => cs.User)
				.HasForeignKey<BankAccount>(ca => ca.UserId);
		}

		public DbSet<User> Users { get; set; }
		public DbSet<BankAccount> BankAccounts { get; set; }
	}
}
