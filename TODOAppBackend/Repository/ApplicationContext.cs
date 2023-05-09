using Microsoft.EntityFrameworkCore;
using TODOAppBackend.Entities;

namespace TODOAppBackend.Repository
{
    public class ApplicationContext : DbContext
    {
        public DbSet<TM_Task> Tasks { get; set; }
        public DbSet<TM_User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
