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
    public class AddClaims : TestBase, IAsyncLifetime
    {

        public AddClaims() : base("MongoUserOnlyStore-AddClaims") { }

        public async Task InitializeAsync()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            await store.CreateAsync(TestUser.First);
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task UpdatesUser()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync(TestIds.UserId1);

            await store.AddClaimsAsync(user,
                new[]
                {
                    new Claim("type","value"),
                    new Claim("type2", "value2")
                });

            user.Claims.Count.ShouldBe(2);
            user.Claims[0].ClaimType.ShouldBe("type");
        }

        [Fact]
        public async Task SavesData()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync(TestIds.UserId1);

            await store.AddClaimsAsync(user,
                new[]
                {
                    new Claim("type","value"),
                    new Claim("type2", "value2")
                });

            await store.UpdateAsync(user);

            context = new TestContext(GetConnection());
            store = new MongoUserOnlyStore<TestUser>(context);
            user = await store.FindByIdAsync(TestIds.UserId1);

            user.Claims.Count.ShouldBe(2);
            user.Claims[0].ClaimType.ShouldBe("type");
        }

        [Fact]
        public async Task ThrowsExceptionWithNullArguments()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync("1000");

            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.AddClaimsAsync(null, new[] { new Claim("type", "value") });
            });
            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.AddClaimsAsync(user, null);
            });
        }

    }
}
