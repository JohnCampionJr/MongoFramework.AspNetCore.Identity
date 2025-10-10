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
    public class RemoveToken : TestBase, IAsyncLifetime
    {

        public RemoveToken() : base("MongoUserOnlyStore-RemoveToken") { }

        public async Task InitializeAsync()
        {

            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            var user = TestUser.First;
            await store.CreateAsync(user);
            await store.SetTokenAsync(user, "provider1", "name1", "token-value1", default);
            await store.SetTokenAsync(user, "provider2", "name2", "token-value2", default);
            await store.UpdateAsync(user);
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task RemovesToken()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync(TestIds.UserId1);

            await store.RemoveTokenAsync(user, "provider2", "name2", default);

            user.Tokens.Count.ShouldBe(1);
        }

        [Fact]
        public async Task SavesData()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync(TestIds.UserId1);

            await store.RemoveTokenAsync(user, "provider2", "name2", default);

            await store.UpdateAsync(user);

            context = new TestContext(GetConnection());
            store = new MongoUserOnlyStore<TestUser>(context);

            user = await store.FindByIdAsync(TestIds.UserId1);

            user.Tokens.Count.ShouldBe(1);
        }

        [Fact]
        public async Task ThrowsExceptionWithNullArguments()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.RemoveTokenAsync(null, "", "", default);
            });
        }
    }
}
