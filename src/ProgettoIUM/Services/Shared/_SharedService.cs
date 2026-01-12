namespace ProgettoIUM.Services.Shared
{
    public partial class SharedService
    {
        ProgettoIUMDbContext _dbContext;

        public SharedService(ProgettoIUMDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
