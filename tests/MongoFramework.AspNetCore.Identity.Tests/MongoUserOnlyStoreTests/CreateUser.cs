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
    public class CreateUser : TestBase
    {

        public CreateUser() : base("MongoUserOnlyStore-CreateUser") { }

        [Fact]
        public async Task ReturnsSuccessWithStringId()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            var result = await store.CreateAsync(TestUser.First);

            result.ShouldBe(IdentityResult.Success);
        }

        [Fact]
        public async Task CreatesDataWithStringId()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            await store.CreateAsync(TestUser.First);

            context.TestUsers.Any().ShouldBeTrue();
            context.TestUsers.Count().ShouldBe(1);
            context.TestUsers.FirstOrDefault()?.CustomData.ShouldBe("Some Info 1");
        }

        [Fact]
        public async Task DoesNotCreatesDataWithAutoSaveOff()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            store.AutoSaveChanges = false;

            await store.CreateAsync(TestUser.First);

            context.TestUsers.Any().ShouldBeFalse();
            context.TestUsers.Count().ShouldBe(0);
        }


        [Fact]
        public async Task ReturnsSuccessWithIntId()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUserInt, DbContext, int>(context);

            var result = await store.CreateAsync(TestUserInt.First);

            result.ShouldBe(IdentityResult.Success);
        }

        [Fact]
        public async Task CreatesDataWithIntId()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUserInt, DbContext, int>(context);

            await store.CreateAsync(TestUserInt.First);

            context.TestUsersInt.Any().ShouldBeTrue();
            context.TestUsersInt.Count().ShouldBe(1);
            context.TestUsersInt.FirstOrDefault()?.CustomData.ShouldBe("Some Info 1");
        }

        [Fact]
        public async Task ThrowsExceptionWithNull()
        {
            var context = new TestContext(GetConnection());
            var store = new MongoUserOnlyStore<TestUser>(context);

            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await store.CreateAsync(null);
            });
        }

    }
}
