using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoEntityFramework.AspNetCore.Identity.Tests.TestClasses;
using Shouldly;
using Xunit;

namespace MongoEntityFramework.AspNetCore.Identity.Tests.MongoUserOnlyStoreTests
{
    public class FindById : TestBase, IAsyncLifetime
    {

        public FindById() : base("MongoUserOnlyStore-FindById") { }

        public async Task InitializeAsync()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var store2 = new MongoUserOnlyStore<TestUserInt, DbContext, int>(context);

            await store.CreateAsync(TestUser.First);
            await store.CreateAsync(TestUser.Second);
            await store.CreateAsync(TestUser.Third);
            await store2.CreateAsync(TestUserInt.First);
            await store2.CreateAsync(TestUserInt.Second);
            await store2.CreateAsync(TestUserInt.Third);
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task FindsCorrectUserWithValidStringId()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            var result = await store.FindByIdAsync(TestIds.UserId2);

            result.ShouldNotBeNull();
            result.UserName.ShouldBe("User Name2");
        }

        [Fact]
        public async Task ReturnsNullWithInvalidStringId()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            var result = await store.FindByIdAsync("none");

            result.ShouldBeNull();
        }

        [Fact]
        public async Task FindsCorrectUserWithValidIntId()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUserInt, DbContext, int>(context);

            var result = await store.FindByIdAsync("2000");

            result.ShouldNotBeNull();
            result.UserName.ShouldBe("User Name2");
        }

        [Fact]
        public async Task ReturnsNullWithInvalidIntId()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUserInt, DbContext, int>(context);

            var result = await store.FindByIdAsync("1234");

            result.ShouldBeNull();
        }

        [Fact]
        public async Task ThrowsExceptionWithNull()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.FindByIdAsync(null);
            });
        }

    }
}
