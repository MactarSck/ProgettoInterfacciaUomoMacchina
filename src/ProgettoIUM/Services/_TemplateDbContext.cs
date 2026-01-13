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
        public DbSet<Comunicazione> Comunicazioni { get; set; }
        public DbSet<StoricoStato> StoricoStati { get; set; }
        public DbSet<Segnalazione> Segnalazioni { get; set; }

    }
}
