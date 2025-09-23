using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace MongoFramework.AspNetCore.Identity.Tests
{
    public class MongoIdentityBuilderExtensionsTests : TestBase
    {

        public MongoIdentityBuilderExtensionsTests() : base("MongoIdentityBuilderExtensionsTests") { }

        [Fact]
        public void RegistersFullyTypedUserStoreWithMongoIdentityContext()
        {
            var services = new ServiceCollection();

            services.AddTransient<IMongoDbConnection>(s =>
            {
                var connection = GetConnection();
                return connection;
            });
            services.AddTransient<MongoIdentityDbContext, MongoIdentityDbContext>();


            services
                .AddIdentity<MongoIdentityUser, MongoIdentityRole>()
                .AddMongoFrameworkStores<MongoIdentityDbContext>();

            var provider = services.BuildServiceProvider();

            using (var scoped = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var db = scoped.ServiceProvider.GetRequiredService<IUserStore<MongoIdentityUser>>())
            {
                db.GetType().GenericTypeArguments.Length.ShouldBe(9);
                db.ShouldBeOfType<MongoUserStore<MongoIdentityUser, MongoIdentityRole, MongoIdentityDbContext, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>>();
            }

        }

        [Fact]
        public void RegistersFullyTypedUserOnlyStoreWithMongoIdentityContext()
        {
            var services = new ServiceCollection();

            services.AddTransient<IMongoDbConnection>(s =>
            {
                var connection = GetConnection();
                return connection;
            });
            services.AddTransient<MongoIdentityDbContext, MongoIdentityDbContext>();


            services
                .AddIdentityCore<MongoIdentityUser>(o => { })
                .AddMongoFrameworkStores<MongoIdentityDbContext>();

            var provider = services.BuildServiceProvider();

            using (var scoped = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var db = scoped.ServiceProvider.GetRequiredService<IUserStore<MongoIdentityUser>>())
            {
                db.GetType().GenericTypeArguments.Length.ShouldBe(6);
                db.ShouldBeOfType<MongoUserOnlyStore<MongoIdentityUser, MongoIdentityDbContext, string, IdentityUserClaim<string>, IdentityUserLogin<string>, IdentityUserToken<string>>>();
            }

        }
        [Fact]
        public void RegistersLimitedTypedUserStoreWithMongoContext()
        {
            var services = new ServiceCollection();

            //need connection
            services.AddTransient<IMongoDbConnection>(s =>
            {
                var connection = GetConnection();
                return connection;
            });

            //need context
            services.AddTransient<MongoDbContext, MongoDbContext>();

            //then we can add the stores
            services
                .AddIdentity<MongoIdentityUser, MongoIdentityRole>()
                .AddMongoFrameworkStores<MongoDbContext>();

            var provider = services.BuildServiceProvider();

            using (var scoped = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var db = scoped.ServiceProvider.GetRequiredService<IUserStore<MongoIdentityUser>>())
            {
                db.GetType().GenericTypeArguments.Length.ShouldBe(4);
                db.ShouldBeOfType<MongoUserStore<MongoIdentityUser, MongoIdentityRole, MongoDbContext, string>>();
            }

        }

        [Fact]
        public void RegistersLimitedTypedUserOnlyStoreWithMongoContext()
        {
            var services = new ServiceCollection();

            services.AddTransient<IMongoDbConnection>(s =>
            {
                var connection = GetConnection();
                return connection;
            });
            services.AddTransient<MongoDbContext, MongoDbContext>();


            services
                .AddIdentityCore<MongoIdentityUser>(o => { })
                .AddMongoFrameworkStores<MongoDbContext>();

            var provider = services.BuildServiceProvider();

            using (var scoped = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var db = scoped.ServiceProvider.GetRequiredService<IUserStore<MongoIdentityUser>>())
            {
                db.GetType().GenericTypeArguments.Length.ShouldBe(3);
                db.ShouldBeOfType<MongoUserOnlyStore<MongoIdentityUser, MongoDbContext, string>>();
            }

        }

        [Fact]
        public void ExtensionThrowsExceptionWithoutMongoIdentityRole()
        {
            var services = new ServiceCollection();

            services.AddTransient<IMongoDbConnection>(s =>
            {
                var connection = GetConnection();
                return connection;
            });
            services.AddTransient<MongoIdentityDbContext, MongoIdentityDbContext>();


            Should.Throw<InvalidOperationException>(() =>
            {
                services
                    .AddIdentity<MongoIdentityUser, IdentityRole>()
                    .AddMongoFrameworkStores<MongoIdentityDbContext>();
            });
        }

        [Fact]
        public void ExtensionThrowsExceptionWithoutMongoIdentityUser()
        {
            var services = new ServiceCollection();

            services.AddTransient<IMongoDbConnection>(s =>
            {
                var connection = GetConnection();
                return connection;
            });
            services.AddTransient<MongoIdentityDbContext, MongoIdentityDbContext>();


            Should.Throw<InvalidOperationException>(() =>
            {
                services
                    .AddIdentity<IdentityUser, MongoIdentityRole>()
                    .AddMongoFrameworkStores<MongoIdentityDbContext>();
            });
        }


    }
}
