using System;
using System.Linq;
using System.Threading.Tasks;
using MongoFramework.AspNetCore.Identity.Tests.TestClasses;
using Shouldly;
using Xunit;

namespace MongoFramework.AspNetCore.Identity.Tests
{
	public class MongoIdentityContextsTests : TestBase, IAsyncLifetime
	{

		public MongoIdentityContextsTests() : base("MongoIdentityContexts") { }

		public async Task InitializeAsync()
		{
			var context = new TestContext(GetConnection());
			var store = new MongoUserStore<TestUser>(context);

            context.Roles.Add(new MongoIdentityRole() {Id = "rid1", Name = "Role 1"});
            context.Roles.Add(new MongoIdentityRole() {Id = "rid2", Name = "Role 2"});
            context.Roles.Add(new MongoIdentityRole() {Id = "rid3", Name = "Role 3"});

            await context.SaveChangesAsync();

			var user = TestUser.First;
            user.Roles.Add("rid1");
            user.Roles.Add("rid2");
			await store.CreateAsync(user);

		}

		public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public void ContextWithRolesLoadsRoles()
        {
            var context = new MongoIdentityDbContext(GetConnection());

            var store = new MongoRoleStore<MongoIdentityRole,MongoIdentityDbContext>(context);

            store.Context.ShouldBeOfType<MongoIdentityDbContext>();
            store.Roles.Count().ShouldBe(3);
        }

        [Fact]
        public void ContextWithUsersLoadsUsers()
        {
            var context = new MongoIdentityUserContext(GetConnection());

            var store = new MongoUserOnlyStore<MongoIdentityUser,MongoIdentityUserContext>(context);

            store.Context.ShouldBeOfType<MongoIdentityDbContext>();
            store.Users.Count().ShouldBe(1);
        }

	}}
