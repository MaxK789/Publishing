using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Publishing.Infrastructure
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly Migrations.PublishingDbContext _ctx;
        private readonly ILogger<DatabaseInitializer> _log;

        public DatabaseInitializer(Migrations.PublishingDbContext ctx, ILogger<DatabaseInitializer> log)
        {
            _ctx = ctx;
            _log = log;
        }

        public void Initialize()
        {
            _log.LogInformation("Applying EF Core migrations...");
            _ctx.Database.Migrate();
            _log.LogInformation("Database schema is up to date.");
        }
    }
}
