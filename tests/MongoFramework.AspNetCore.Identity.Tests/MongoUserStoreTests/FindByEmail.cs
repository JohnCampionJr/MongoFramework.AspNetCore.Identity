using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoEntityFramework.AspNetCore.Identity.Tests.TestClasses;
using Shouldly;
using Xunit;

namespace MongoEntityFramework.AspNetCore.Identity.Tests.MongoUserStoreTests
{
    public class FindByEmail : TestBase, IAsyncLifetime
    {

        public FindByEmail() : base("MongoUserStore-FindByEmail") { }

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
        public async Task FindsCorrectUserWithValidEmail()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserStore<TestUser>(context);

            var result = await store.FindByEmailAsync("TEST3@TESTING.COM");

            result.ShouldNotBeNull();
            result.UserName.ShouldBe("User Name3");
        }

        [Fact]
        public async Task FindsTrackedEntityWithValidEmail()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserStore<TestUser>(context);
            var tracked = await store.FindByIdAsync(TestIds.UserId2);
            tracked.CustomData = "updated";

            var result = await store.FindByEmailAsync("TEST2@TESTING.COM");

            result.ShouldBeSameAs(tracked);
            result.CustomData.ShouldBe("updated");
        }

        [Fact]
        public async Task ReturnsNullWithInvalidEmail()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserStore<TestUser>(context);

            var result = await store.FindByEmailAsync("none");

            result.ShouldBeNull();
        }

        [Fact]
        public async Task ThrowsExceptionWithNull()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserStore<TestUser>(context);

            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.FindByEmailAsync(null);
            });
        }

    }
}
