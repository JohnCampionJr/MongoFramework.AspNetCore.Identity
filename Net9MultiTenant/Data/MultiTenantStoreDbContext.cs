using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using Microsoft.EntityFrameworkCore;
using Net9MultiTenant.Models;

namespace Net9MultiTenant.Data
{
    public class MultiTenantStoreDbContext : EFCoreStoreDbContext<TenantInfo>
    {
        public MultiTenantStoreDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // If necessary call the base class method.
            // Recommended to be called first.
            base.OnModelCreating(builder);
            Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use InMemory, but could be MsSql, Sqlite, MySql, etc...
            //optionsBuilder.UseMongoDB("mongodb://localhost/fb9-tenant-store");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
