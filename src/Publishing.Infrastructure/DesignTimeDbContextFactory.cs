using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Publishing.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            // Use local SQL Server for design-time services. Replace with actual connection if needed.
            builder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Publishing;Trusted_Connection=True;",
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
            return new AppDbContext(builder.Options);
        }
    }
}
