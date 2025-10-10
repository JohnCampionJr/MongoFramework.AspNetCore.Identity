using System.Reflection.Emit;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Metadata.Conventions;
using MongoEntityFramework.AspNetCore.Identity;
using Net9MultiTenant.Models;

namespace Net9MultiTenant.Data
{
    public class ApplicationBaseDbContext(IMultiTenantContextAccessor multiTenantContextAccessor, DbContextOptions<ApplicationBaseDbContext> options) : MultiTenantMongoIdentityDbContext(multiTenantContextAccessor, options)
    {

        public DbSet<ToDoItem> ToDoItems { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new CamelCaseElementNameConvention());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // If necessary call the base class method.
            // Recommended to be called first.
            base.OnModelCreating(builder);

            builder.Entity<ToDoItem>().IsMultiTenant();

        }

    }
}
