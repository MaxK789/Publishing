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
            return _context.Database.MigrateAsync();
        }
    }
}
