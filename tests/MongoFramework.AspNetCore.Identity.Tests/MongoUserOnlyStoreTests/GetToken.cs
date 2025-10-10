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
    public class GetToken : TestBase, IAsyncLifetime
    {

        public GetToken() : base("MongoUserOnlyStore-GetToken") { }

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
        public async Task RetrieveLoginsFromUser()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync(TestIds.UserId1);

            var token1 = await store.GetTokenAsync(user, "provider1", "name1", default);
            var token2 = await store.GetTokenAsync(user, "provider2", "name2", default);

            token1.ShouldBe("token-value1");
            token2.ShouldBe("token-value2");
        }

        [Fact]
        public async Task ThrowsExceptionWithNullArguments()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.GetTokenAsync(null, "", "", default);
            });
        }

    }
}
