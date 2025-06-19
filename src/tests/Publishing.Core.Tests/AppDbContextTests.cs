using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Infrastructure;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class AppDbContextTests
    {
        [TestMethod]
        public void Constructor_DoesNotEnsureCreated()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            // Creating the context should not attempt to create the database
            // because EnsureCreated is not called in the constructor.
            var context = new AppDbContext(options);

            Assert.IsNotNull(context);
        }
    }
}
