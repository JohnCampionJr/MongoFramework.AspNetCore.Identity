using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoEntityFramework.AspNetCore.Identity.Tests.TestClasses;
using Shouldly;
using Xunit;

namespace MongoEntityFramework.AspNetCore.Identity.Tests.MongoUserOnlyStoreTests
{
    public class DeleteUser : TestBase
    {

        public DeleteUser() : base("MongoUserOnlyStore-DeleteUser") { }

        [Fact]
        public async Task DeletesDataWithValidUser()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            await store.CreateAsync(TestUser.First);

            context.TestUsers.Any().ShouldBeTrue();

            context = new TestContext(GetConnection());
            store = new MongoUserOnlyStore<TestUser>(context);
            var user = await context.TestUsers.FirstOrDefaultAsync();

            await store.DeleteAsync(user);

            context.TestUsers.Any().ShouldBeFalse();
        }

        [Fact]
        public async Task ReturnsSuccessWithValidUser()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);
            await store.CreateAsync(TestUser.First);

            context.TestUsers.Any().ShouldBeTrue();

            context = new TestContext(GetConnection());
            store = new MongoUserOnlyStore<TestUser>(context);
            var user = await context.TestUsers.FirstOrDefaultAsync();

            var result = await store.DeleteAsync(user);

            result.ShouldBe(IdentityResult.Success);
        }

        [Fact]
        public async Task ThrowsExceptionWithNull()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.DeleteAsync(null);
            });
        }

    }
}
