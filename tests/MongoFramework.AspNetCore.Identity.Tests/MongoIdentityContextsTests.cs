using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoEntityFramework.AspNetCore.Identity.Tests.TestClasses;
using Shouldly;
using Xunit;

namespace MongoEntityFramework.AspNetCore.Identity.Tests
{
    public static class TestIds
    {
        public static readonly string RoleId1 = ObjectId.GenerateNewId().ToString();
        public static readonly string RoleId2 = ObjectId.GenerateNewId().ToString();
        public static readonly string RoleId3 = ObjectId.GenerateNewId().ToString();
        public static readonly string UserId1 = ObjectId.GenerateNewId().ToString();
        public static readonly string UserId2 = ObjectId.GenerateNewId().ToString();
        public static readonly string UserId3 = ObjectId.GenerateNewId().ToString();
    }
    public class MongoIdentityContextsTests : TestBase, IAsyncLifetime
    {

        public MongoIdentityContextsTests() : base("MongoIdentityContexts") { }

        public async Task InitializeAsync()
        {
            var context = new MongoIdentityDbContext(GetConnection());
            var store = new MongoUserStore<MongoIdentityUser>(context);
            
            context.Roles.Add(new MongoIdentityRole { Id = TestIds.RoleId1, Name = "Role 1" });
            context.Roles.Add(new MongoIdentityRole { Id = TestIds.RoleId2, Name = "Role 2" });
            context.Roles.Add(new MongoIdentityRole { Id = TestIds.RoleId3, Name = "Role 3" });

            await context.SaveChangesAsync();

            var user = TestUser.First;
            user.Roles.Add(TestIds.RoleId1);
            user.Roles.Add(TestIds.RoleId2);
            await store.CreateAsync(user);

        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public void ContextWithRolesLoadsRoles()
        {
            var context = new MongoIdentityDbContext(GetConnection());

            var store = new MongoRoleStore<MongoIdentityRole, MongoIdentityDbContext>(context);

            store.Context.ShouldBeOfType<MongoIdentityDbContext>();
            store.Roles.Count().ShouldBe(3);
        }

        [Fact]
        public void ContextWithUsersLoadsUsers()
        {
            var context = new MongoIdentityUserContext(GetConnection());

            var store = new MongoUserOnlyStore<MongoIdentityUser, MongoIdentityUserContext>(context);

            store.Context.ShouldBeOfType<MongoIdentityUserContext>();
            store.Users.Count().ShouldBe(1);
        }

    }
}
