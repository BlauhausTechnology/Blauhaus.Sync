using Microsoft.EntityFrameworkCore;

namespace Blauhaus.Sync.Tests.Server.TestObjects
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() { }

        public  MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<MyServerEntity> MyServerEntities { get; set; } = null!;
    }
}