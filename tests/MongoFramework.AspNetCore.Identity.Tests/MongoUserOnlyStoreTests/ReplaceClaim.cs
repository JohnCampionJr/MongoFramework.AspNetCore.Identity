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
    public class ReplaceClaim : TestBase, IAsyncLifetime
    {

        public ReplaceClaim() : base("MongoUserOnlyStore-ReplaceClaim") { }

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
        public async Task ReplaceUsersClaim()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync(TestIds.UserId1);

            var claims = await store.GetClaimsAsync(user);
            var claim = claims[0];

            await store.ReplaceClaimAsync(user, claim, new Claim("new-type", "new-value"));

            user.Claims[0].ClaimType.ShouldBe("new-type");
            user.Claims[0].ClaimValue.ShouldBe("new-value");
        }

        [Fact]
        public async Task SavesData()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync(TestIds.UserId1);

            var claims = await store.GetClaimsAsync(user);
            var claim = claims[0];

            await store.ReplaceClaimAsync(user, claim, new Claim("new-type", "new-value"));

            await store.UpdateAsync(user);

            context = new TestContext(GetConnection());
            store = new MongoUserOnlyStore<TestUser>(context);
            user = await store.FindByIdAsync(TestIds.UserId1);

            user.Claims.Count.ShouldBe(2);
            user.Claims[0].ClaimType.ShouldBe("new-type");
        }

        [Fact]
        public async Task ThrowsExceptionWithNullArguments()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync("1000");

            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.ReplaceClaimAsync(null, new Claim("type", "value"), new Claim("type", "value"));
            });
            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.ReplaceClaimAsync(user, null, new Claim("type", "value"));
            });
            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.ReplaceClaimAsync(user, new Claim("type", "value"), null);
            });
        }

    }
}
