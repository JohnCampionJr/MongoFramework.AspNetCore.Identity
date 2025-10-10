using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoEntityFramework.AspNetCore.Identity.Tests.TestClasses;
using Shouldly;
using Xunit;

namespace MongoEntityFramework.AspNetCore.Identity.Tests.MongoUserOnlyStoreTests
{
    public class SetToken : TestBase, IAsyncLifetime
    {

        public SetToken() : base("MongoUserOnlyStore-SetToken") { }

        public async Task InitializeAsync()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            await store.CreateAsync(TestUser.First);
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task UpdatesUserWithValidData()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync(TestIds.UserId1);

            await store.SetTokenAsync(user, "provider", "name", "token-value", default);

            user.Tokens.Count.ShouldBe(1);
            user.Tokens[0].Value.ShouldBe("token-value");
        }

        [Fact]
        public async Task SavesData()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync(TestIds.UserId1);

            await store.SetTokenAsync(user, "provider", "name", "token-value", default);

            await store.UpdateAsync(user);

            context = new TestContext(GetConnection());
            store = new MongoUserOnlyStore<TestUser>(context);
            user = await store.FindByIdAsync(TestIds.UserId1);

            user.Tokens.Count.ShouldBe(1);
            user.Tokens[0].Value.ShouldBe("token-value");
        }

        [Fact]
        public async Task ThrowsExceptionWithNullArguments()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.SetTokenAsync(null, "", "", "", default);
            });
        }

    }
}
