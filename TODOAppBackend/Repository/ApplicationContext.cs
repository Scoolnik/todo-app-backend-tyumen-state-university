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
    }
}
