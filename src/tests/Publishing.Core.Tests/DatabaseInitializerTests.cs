using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Infrastructure;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class DatabaseInitializerTests
    {
        [TestMethod]
        public void InitializeAsync_WhenUpToDate_DoesNotThrow()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            using var context = new AppDbContext(options);
            context.Database.OpenConnection();
            var initializer = new DatabaseInitializer(context);

            initializer.InitializeAsync().GetAwaiter().GetResult();
            // Second call should not throw if the schema is already up to date.
            initializer.InitializeAsync().GetAwaiter().GetResult();
        }
    }
}
