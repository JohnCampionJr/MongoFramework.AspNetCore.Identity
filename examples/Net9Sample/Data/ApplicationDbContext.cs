using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Metadata.Conventions;
using MongoEntityFramework.AspNetCore.Identity;

namespace Net9Sample.Data
{
    public class ApplicationDbContext : MongoIdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new CamelCaseElementNameConvention());
        }

    }
}
