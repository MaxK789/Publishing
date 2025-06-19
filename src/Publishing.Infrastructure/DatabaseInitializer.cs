using Microsoft.EntityFrameworkCore;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly AppDbContext _context;

        public DatabaseInitializer(AppDbContext context)
        {
            _context = context;
        }

        public Task InitializeAsync()
        {
            // Apply pending migrations if any exist. This keeps the database
            // schema in sync with the current EF Core model.
            return _context.Database.MigrateAsync();
        }
    }
}
