using Microsoft.EntityFrameworkCore;
using ProgettoIUM.Infrastructure;
using ProgettoIUM.Services.Shared;

namespace ProgettoIUM.Services
{
    public class ProgettoIUMDbContext : DbContext
    {
        public ProgettoIUMDbContext()
        {
        }

        public ProgettoIUMDbContext(DbContextOptions<ProgettoIUMDbContext> options) : base(options)
        {
            DataGenerator.InitializeUsers(this);
        }

        public DbSet<User> Users { get; set; }
    }
}
