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
    public class MongoFrameworkServiceCollectionExtensionsTests : TestBase
    {
        public MongoFrameworkServiceCollectionExtensionsTests() : base("MongoFrameworkServiceCollectionExtensionsTests") { }

        [Fact]
        public void RegistersContextWithNoParameters()
        {
            var services = new ServiceCollection();

            var mainContextMongoUri = new MongoDB.Driver.MongoUrl("mongodb://localhost/identity-test");
            services.AddTransient<IMongoDbConnection>(s =>
            {
                var connection = MongoDbConnection.FromUrl(mainContextMongoUri);
                return connection;
            });

            services.AddDbContext<MongoIdentityDbContext>();

            var provider = services.BuildServiceProvider();

            using (var scoped = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var db = scoped.ServiceProvider.GetRequiredService<MongoIdentityDbContext>())
            {
                db.ShouldBeOfType<MongoIdentityDbContext>();
                db.Connection.ShouldNotBeNull();
                db.Connection.GetDatabase().DatabaseNamespace.DatabaseName.ShouldBe("identity-test");
            }

        }

        [Fact]
        public void RegistersConnectionWithValidConnectionString()
        {
            var services = new ServiceCollection();

            services.AddMongoDbContext<MongoIdentityDbContext>(x =>
            {
                x.ConnectionString = "mongodb://localhost/identity-test";
            });

            var provider = services.BuildServiceProvider();

            using (var scoped = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var db = scoped.ServiceProvider.GetRequiredService<MongoIdentityDbContext>())
            {
                db.ShouldBeOfType<MongoIdentityDbContext>();
                db.Connection.ShouldNotBeNull();
                db.Connection.GetDatabase().DatabaseNamespace.DatabaseName.ShouldBe("identity-test");
            }
        }

        [Fact]
        public void RegistersConnectionWithListener()
        {
            var services = new ServiceCollection();

            services.AddMongoDbContext<MongoIdentityDbContext>(x =>
            {
                x.ConnectionString = "mongodb://localhost/identity-test";
                x.DiagnosticListener = new NoOpDiagnosticListener();
            });

            var provider = services.BuildServiceProvider();

            using (var scoped = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var db = scoped.ServiceProvider.GetRequiredService<MongoIdentityDbContext>())
            {
                db.Connection.DiagnosticListener.ShouldBeOfType<NoOpDiagnosticListener>();
            }
        }
    }
}
