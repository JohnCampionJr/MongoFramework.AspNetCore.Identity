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
    public class GetClaims : TestBase, IAsyncLifetime
    {

        public GetClaims() : base("MongoUserOnlyStore-GetClaims") { }

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
        public async Task RetrievesClaimsFromUser()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync(TestIds.UserId1);

            var claims = await store.GetClaimsAsync(user);

            claims.Count.ShouldBe(2);
            claims[0].Type.ShouldBe("type");
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
