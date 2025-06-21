using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Infrastructure;
using System.Threading.Tasks;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class DatabaseInitializerTests
    {
        [TestMethod]
        public async Task InitializeAsync_WhenUpToDate_DoesNotThrow()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            using var context = new AppDbContext(options);
            context.Database.OpenConnection();
            var initializer = new DatabaseInitializer(context);

            await initializer.InitializeAsync();
            // Second call should not throw if the schema is already up to date.
            await initializer.InitializeAsync();
        }
    }
}
