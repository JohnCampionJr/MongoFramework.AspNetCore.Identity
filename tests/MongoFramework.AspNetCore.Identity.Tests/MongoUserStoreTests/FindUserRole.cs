using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoFramework.AspNetCore.Identity.Tests.TestClasses;
using Shouldly;
using Xunit;

namespace MongoFramework.AspNetCore.Identity.Tests.MongoUserStoreTests
{
	public class FindUserRole : TestBase, IAsyncLifetime
	{
        private class TestStore : MongoUserStore<TestUser>
        {
            public TestStore(MongoDbContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }

            public async Task<IdentityUserRole<string>> ExposeFindUserRoleAsync(string userId, string roleId)
            {
                return await base.FindUserRoleAsync(userId, roleId, default);
            }
        }

		public FindUserRole() : base("MongoUserStore-FindUserRole") { }

		public async Task InitializeAsync()
		{
			var context = new TestContext(GetConnection());
			var store = new MongoUserStore<TestUser>(context);

            context.Roles.Add(new MongoIdentityRole {Id = "rid1", Name = "Role 1"});
            context.Roles.Add(new MongoIdentityRole {Id = "rid2", Name = "Role 2"});
            context.Roles.Add(new MongoIdentityRole {Id = "rid3", Name = "Role 3"});

            await context.SaveChangesAsync();

			var user = TestUser.First;
            user.Roles.Add("rid1");
            user.Roles.Add("rid2");
			await store.CreateAsync(user);

		}

		public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task FindUserRoleWithValidRole()
        {
            var context = new TestContext(GetConnection());
            var store = new TestStore(context);

            var role = await store.ExposeFindUserRoleAsync("a1","rid1");

            role.ShouldNotBeNull();
            role.RoleId.ShouldBe("rid1");
            role.UserId.ShouldBe("a1");
        }

        [Fact]
        public async Task FindUserRoleFailsWithInvalidRole()
        {
            var context = new TestContext(GetConnection());
            var store = new TestStore(context);

            var role = await store.ExposeFindUserRoleAsync("a1","none-rid1");

            role.ShouldBeNull();
        }

        [Fact]
        public async Task FindUserRoleFailsWithInvaliUser()
        {
            var context = new TestContext(GetConnection());
            var store = new TestStore(context);

            var role = await store.ExposeFindUserRoleAsync("none-a1","rid1");

            role.ShouldBeNull();
        }

		[Fact]
		public async Task ThrowsExceptionWithNullArguments()
		{
			var context = new TestContext(GetConnection());
			var store = new TestStore(context);

            await Should.ThrowAsync<ArgumentNullException>( async () =>
            {
                await store.ExposeFindUserRoleAsync(null, "rid1");
            });
            await Should.ThrowAsync<ArgumentNullException>( async () =>
            {
                await store.ExposeFindUserRoleAsync("a1", null);
            });
		}

	}}
