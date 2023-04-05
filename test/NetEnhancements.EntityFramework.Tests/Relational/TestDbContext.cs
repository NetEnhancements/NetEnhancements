using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace NetEnhancements.EntityFramework.Tests.Relational
{
    internal class InMemoryTestDbContext : DbContext
    {
        public DbSet<TestEntity> TestEntities { get; set; }

#pragma warning disable CS8618
        public InMemoryTestDbContext()
#pragma warning restore CS8618
            : base(new DbContextOptionsBuilder<InMemoryTestDbContext>()
                .UseInMemoryDatabase("Test")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
