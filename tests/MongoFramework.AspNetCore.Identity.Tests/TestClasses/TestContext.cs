using Microsoft.EntityFrameworkCore;

namespace MongoEntityFramework.AspNetCore.Identity.Tests.TestClasses
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            this.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
        }

        public DbSet<TestUser> TestUsers { get; set; }
        public DbSet<TestUserInt> TestUsersInt { get; set; }
        public DbSet<MongoIdentityRole> Roles { get; set; }
    }
}
