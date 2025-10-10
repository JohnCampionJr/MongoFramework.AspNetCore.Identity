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
    public class GetUsersForClaim : TestBase, IAsyncLifetime
    {

        public GetUsersForClaim() : base("MongoUserOnlyStore-GetUsersForClaim") { }

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
            user = TestUser.Second;
            await store.CreateAsync(user);
            await store.AddClaimsAsync(user,
                new[]
                {
                    new Claim("type","value"),
                    new Claim("type2", "value2")
                });
            await store.UpdateAsync(user);
            user = TestUser.Third;
            await store.CreateAsync(user);
            await store.AddClaimsAsync(user,
                new[]
                {
                    new Claim("type","value"),
                    new Claim("type3", "value3")
                });
            await store.UpdateAsync(user);

        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task RetrieveUsersForClaim()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            var users = await store.GetUsersForClaimAsync(new Claim("type", "value"));
            var users2 = await store.GetUsersForClaimAsync(new Claim("type2", "value2"));

            users.Count.ShouldBe(3);
            users2.Count.ShouldBe(2);

        }

        [Fact]
        public async Task ThrowsExceptionWithNullArguments()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.GetClaimsAsync(null);
            });
        }

    }
}
