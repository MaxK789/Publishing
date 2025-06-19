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
            // For simpler integration testing we ensure the schema exists
            // rather than running migrations. This works because the model is
            // kept in sync with EF Core 6.
            return _context.Database.EnsureCreatedAsync();
        }
    }
}
