using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Publishing.Infrastructure.Migrations
{
    [DbContext(typeof(PublishingDbContext))]
    public class PublishingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            // Model snapshot intentionally minimal for migrations
        }
    }
}
