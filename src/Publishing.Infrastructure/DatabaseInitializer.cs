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
            // Use EnsureCreated so the initializer can run safely against an
            // existing database. It creates the schema only if the tables are
            // missing and is idempotent across multiple calls.
            return _context.Database.EnsureCreatedAsync();
        }
    }
}
