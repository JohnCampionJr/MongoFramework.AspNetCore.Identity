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
    public class RemoveClaim : TestBase, IAsyncLifetime
    {

        public RemoveClaim() : base("MongoUserOnlyStore-RemoveClaim") { }

        public async Task InitializeAsync()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            var user = TestUser.First;
            await store.CreateAsync(user);
            await store.AddClaimsAsync(user,
                new[]
                {
                    new Claim("type","value"),
                    new Claim("type2", "value2")
                });
            await store.UpdateAsync(user);

        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task RemoveClaimWithExistingClaim()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync(TestIds.UserId1);

            var claims = await store.GetClaimsAsync(user);

            await store.RemoveClaimsAsync(user, claims);

            user.Claims.Count.ShouldBe(0);
        }

        [Fact]
        public async Task RemoveClaimWithNewClaim()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync(TestIds.UserId1);

            await store.RemoveClaimsAsync(user, new[] {
                new Claim("type","value"),
                new Claim("type2", "value2")
            });

            user.Claims.Count.ShouldBe(0);
        }

        [Fact]
        public async Task SavesData()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync(TestIds.UserId1);

            var claims = await store.GetClaimsAsync(user);

            await store.RemoveClaimsAsync(user, claims);
            await store.UpdateAsync(user);

            context = new TestContext(GetConnection());
            store = new MongoUserOnlyStore<TestUser>(context);
            user = await store.FindByIdAsync(TestIds.UserId1);

            user.Claims.Count.ShouldBe(0);
        }

        [Fact]
        public async Task ThrowsExceptionWithNullArguments()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync("1000");

            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.RemoveClaimsAsync(null, new[] { new Claim("type", "value") });
            });
            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.RemoveClaimsAsync(user, null);
            });
        }

    }
}
