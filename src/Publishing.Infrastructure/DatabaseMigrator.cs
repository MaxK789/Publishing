using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Publishing.Core.Interfaces;
using Publishing.Migrations;

namespace Publishing.Infrastructure
{
    public class DatabaseMigrator : IDatabaseInitializer
    {
        private readonly IConfiguration _config;

        public DatabaseMigrator(IConfiguration config)
        {
            _config = config;
        }

        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(_config.GetConnectionString("DefaultConnection"))
                .Options;

            using var context = new AppDbContext(options);
            context.Database.Migrate();
        }
    }
}
