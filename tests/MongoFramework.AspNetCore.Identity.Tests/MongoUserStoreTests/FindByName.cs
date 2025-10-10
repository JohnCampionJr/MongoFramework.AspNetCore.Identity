using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoEntityFramework.AspNetCore.Identity.Tests.TestClasses;
using Shouldly;
using Xunit;

namespace MongoEntityFramework.AspNetCore.Identity.Tests.MongoUserStoreTests
{
    public class FindByName : TestBase, IAsyncLifetime
    {

        public FindByName() : base("MongoUserStore-FindByName") { }

        public async Task InitializeAsync()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserStore<TestUser>(context);

            await store.CreateAsync(TestUser.First);
            await store.CreateAsync(TestUser.Second);
            await store.CreateAsync(TestUser.Third);
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task FindsCorrectUserWithValidUserName()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserStore<TestUser>(context);

            var result = await store.FindByNameAsync("USER NAME2");

            result.ShouldNotBeNull();
            result.UserName.ShouldBe("User Name2");
        }


        [Fact]
        public async Task FindsTrackedEntityWithValidUserName()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserStore<TestUser>(context);
            var tracked = await store.FindByIdAsync(TestIds.UserId2);
            tracked.CustomData = "updated";

            var result = await store.FindByNameAsync("USER NAME2");

            result.ShouldBeSameAs(tracked);
            result.CustomData.ShouldBe("updated");
        }


        [Fact]
        public async Task ReturnsNullWithInvalidUserName()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserStore<TestUser>(context);

            var result = await store.FindByNameAsync("none");

            result.ShouldBeNull();
        }

    }
}
