using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoEntityFramework.AspNetCore.Identity.Tests.TestClasses;
using Shouldly;
using Xunit;

namespace MongoEntityFramework.AspNetCore.Identity.Tests.MongoUserOnlyStoreTests
{
    public class UpdateUser : TestBase, IAsyncLifetime
    {

        public UpdateUser() : base("MongoUserOnlyStore-UpdateUser") { }

        public async Task InitializeAsync()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            await store.CreateAsync(TestUser.First);
            await store.CreateAsync(TestUser.Second);
            await store.CreateAsync(TestUser.Third);
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task ReturnsSuccess()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            var user = await store.FindByIdAsync(TestIds.UserId1);

            user.CustomData = "new-data";
            var result = await store.UpdateAsync(user);

            result.ShouldBe(IdentityResult.Success);
        }

        [Fact]
        public async Task SavesData()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            var user = await store.FindByIdAsync(TestIds.UserId1);

            user.CustomData = "new-data";
            await store.UpdateAsync(user);

            context.TestUsers.FirstOrDefault()?.CustomData.ShouldBe("new-data");
        }

        [Fact]
        public async Task ThrowsExceptionWithNull()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.UpdateAsync(null);
            });
        }
    }
}
