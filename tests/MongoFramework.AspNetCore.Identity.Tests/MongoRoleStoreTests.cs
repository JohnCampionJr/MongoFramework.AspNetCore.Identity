using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoEntityFramework.AspNetCore.Identity.Tests.TestClasses;
using Shouldly;
using Xunit;

namespace MongoEntityFramework.AspNetCore.Identity.Tests
{
    public class MongoRoleStoreTests : TestBase, IAsyncLifetime
    {

        public MongoRoleStoreTests() : base("MongoRoleStore") { }

        public async Task InitializeAsync()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserStore<TestUser>(context);

            context.Roles.Add(new MongoIdentityRole { Id = "rid1", Name = "Role 1" });
            context.Roles.Add(new MongoIdentityRole { Id = "rid2", Name = "Role 2" });
            context.Roles.Add(new MongoIdentityRole { Id = "rid3", Name = "Role 3" });

            await context.SaveChangesAsync();

            var user = TestUser.First;
            user.Roles.Add("rid1");
            user.Roles.Add("rid2");
            await store.CreateAsync(user);

        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public void ConstructorUsesMongo()
        {
            var context = new TestContext(GetConnection());

            var store = new MongoRoleStore<MongoIdentityRole>(context);

            store.Context.ShouldBeOfType<TestContext>();
        }

        [Fact]
        public void ConvertIdFromStringWithIntReturnsZero()
        {
            var context = new TestContext(GetConnection());

            var store = new MongoRoleStore<MongoIdentityRole<int>, DbContext, int>(context);

            store.ConvertIdFromString(null).ShouldBe(0);
            store.ConvertIdFromString("12345").ShouldBe(12345);
        }

        [Fact]
        public void ConvertIdToStringWithIntReturnsNull()
        {
            var context = new TestContext(GetConnection());

            var store = new MongoRoleStore<MongoIdentityRole<int>, DbContext, int>(context);

            store.ConvertIdToString(0).ShouldBeNull();
            store.ConvertIdToString(12345).ShouldBe("12345");
        }

        [Fact]
        public async Task GetNormalizedRoleReturnsCorrect()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoRoleStore<MongoIdentityRole<int>, DbContext, int>(context);

            var role = new MongoIdentityRole<int> { Name = "testrole", NormalizedName = "TESTROLE" };
            var name = await store.GetNormalizedRoleNameAsync(role);

            name.ShouldBe("TESTROLE");
        }

    }
}
