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
    public class RemoveLogin : TestBase, IAsyncLifetime
    {

        public RemoveLogin() : base("MongoUserOnlyStore-RemoveLogin") { }

        public async Task InitializeAsync()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            var user = TestUser.First;
            await store.CreateAsync(user);
            await store.AddLoginAsync(user, new UserLoginInfo("provider1", "provider-key", "Login Provider"));
            await store.UpdateAsync(user);

        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task RemoveLoginWithExistingLogin()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync(TestIds.UserId1);

            await store.RemoveLoginAsync(user, "provider1", "provider-key");

            user.Logins.Count.ShouldBe(0);
        }

        [Fact]
        public async Task SavesData()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync(TestIds.UserId1);

            await store.RemoveLoginAsync(user, "provider1", "provider-key");
            await store.UpdateAsync(user);

            context = new TestContext(GetConnection());
            store = new MongoUserOnlyStore<TestUser>(context);
            user = await store.FindByIdAsync(TestIds.UserId1);

            user.Logins.Count.ShouldBe(0);
        }

        [Fact]
        public async Task ThrowsExceptionWithNullArguments()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.RemoveLoginAsync(null, "", "");
            });
        }

    }
}
