using Microsoft.EntityFrameworkCore;
using TODOAppBackend.Entities;

namespace TODOAppBackend.Repository
{
    public class ApplicationContext : DbContext
    {
        public DbSet<TM_Task> TM_Task { get; set; }
        public DbSet<TM_User> TM_User { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			var users = new[]
			{
				new TM_User()
				{
					ID = 1,
					UserLogin = "admin",
					UserPassword = "admin",
				},
				new TM_User()
				{
					ID = 2,
					UserLogin = "string",
					UserPassword = "string",
				}
			};

			modelBuilder.Entity<TM_User>().HasData(users);
		}
	}
}
