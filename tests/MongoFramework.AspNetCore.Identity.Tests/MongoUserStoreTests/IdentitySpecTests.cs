using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace MongoEntityFramework.AspNetCore.Identity.Tests.MongoUserStoreTests
{
    public class IdentitySpecTests : IdentitySpecificationTestBase<MongoIdentityUser, MongoIdentityRole>, IDisposable
    {
        public void Dispose()
        {
            ClearDatabase();
        }

        private void ClearDatabase()
        {
            //Removing the database created for the tests
            var client = new MongoClient(TestConfiguration.ConnectionString);
            client.DropDatabase("MongoUserStore-IdentitySpec");
        }

        private MongoIdentityDbContext CreateContext()
        {
            var conn = TestConfiguration.GetConnection("MongoUserStore-IdentitySpec");
            var db = new MongoIdentityDbContext(conn);
            return db;
        }

        protected override object CreateTestContext()
        {
            return CreateContext();
        }

        protected override void AddUserStore(IServiceCollection services, object context = null)
        {
            services.AddSingleton<IUserStore<MongoIdentityUser>>(new MongoUserStore<MongoIdentityUser, MongoIdentityRole, DbContext>((DbContext)context));
        }

        protected override void SetUserPasswordHash(MongoIdentityUser user, string hashedPassword)
        {
            user.PasswordHash = hashedPassword;
        }

        protected override MongoIdentityUser CreateTestUser(string namePrefix = "", string email = "", string phoneNumber = "",
            bool lockoutEnabled = false, DateTimeOffset? lockoutEnd = null, bool useNamePrefixAsUserName = false)
        {
            return new MongoIdentityUser
            {
                UserName = useNamePrefixAsUserName ? namePrefix : string.Format(CultureInfo.InvariantCulture, "{0}{1}", namePrefix, Guid.NewGuid()),
                Email = email,
                PhoneNumber = phoneNumber,
                LockoutEnabled = lockoutEnabled,
                LockoutEnd = lockoutEnd
            };
        }

        protected override Expression<Func<MongoIdentityUser, bool>> UserNameEqualsPredicate(string userName) => u => u.UserName == userName;

        protected override Expression<Func<MongoIdentityRole, bool>> RoleNameEqualsPredicate(string roleName) => r => r.Name == roleName;

        protected override Expression<Func<MongoIdentityRole, bool>> RoleNameStartsWithPredicate(string roleName) => r => r.Name.StartsWith(roleName);

        protected override Expression<Func<MongoIdentityUser, bool>> UserNameStartsWithPredicate(string userName) => u => u.UserName.StartsWith(userName);

        protected override void AddRoleStore(IServiceCollection services, object context = null)
        {
            services.AddSingleton<IRoleStore<MongoIdentityRole>>(new MongoRoleStore<MongoIdentityRole, DbContext>((DbContext)context));
        }

        protected override MongoIdentityRole CreateTestRole(string roleNamePrefix = "", bool useRoleNamePrefixAsRoleName = false)
        {
            var roleName = useRoleNamePrefixAsRoleName ? roleNamePrefix : string.Format(CultureInfo.InvariantCulture, "{0}{1}", roleNamePrefix, Guid.NewGuid());
            return new MongoIdentityRole(roleName);
        }

        [Fact]
        public async Task SqlUserStoreMethodsThrowWhenDisposedTest()
        {
            var store = new MongoUserStore(CreateContext());
            store.Dispose();
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.AddClaimsAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.AddLoginAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.AddToRoleAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.GetClaimsAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.GetLoginsAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.GetRolesAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.IsInRoleAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.RemoveClaimsAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.RemoveLoginAsync(null, null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(
                async () => await store.RemoveFromRoleAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.RemoveClaimsAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.ReplaceClaimAsync(null, null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.FindByLoginAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.FindByIdAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.FindByNameAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.CreateAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.UpdateAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.DeleteAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(
                async () => await store.SetEmailConfirmedAsync(null, true));
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.GetEmailConfirmedAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(
                async () => await store.SetPhoneNumberConfirmedAsync(null, true));
            await Assert.ThrowsAsync<ObjectDisposedException>(
                async () => await store.GetPhoneNumberConfirmedAsync(null));
        }

        [Fact]
        public async Task TwoUsersSamePasswordDifferentHash()
        {
            var manager = CreateManager();
            var userA = new MongoIdentityUser(Guid.NewGuid().ToString());
            IdentityResultAssert.IsSuccess(await manager.CreateAsync(userA, "password"));
            var userB = new MongoIdentityUser(Guid.NewGuid().ToString());
            IdentityResultAssert.IsSuccess(await manager.CreateAsync(userB, "password"));

            Assert.NotEqual(userA.PasswordHash, userB.PasswordHash);
        }

        [Fact]
        public async Task FindByEmailThrowsWithTwoUsersWithSameEmail()
        {
            var manager = CreateManager();
            var userA = new MongoIdentityUser(Guid.NewGuid().ToString());
            userA.Email = "dupe@dupe.com";
            IdentityResultAssert.IsSuccess(await manager.CreateAsync(userA, "password"));
            var userB = new MongoIdentityUser(Guid.NewGuid().ToString());
            userB.Email = "dupe@dupe.com";
            IdentityResultAssert.IsSuccess(await manager.CreateAsync(userB, "password"));
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await manager.FindByEmailAsync("dupe@dupe.com"));
        }

        [Fact]
        public async Task AddUserToUnknownRoleFails()
        {
            var manager = CreateManager();
            var u = CreateTestUser();
            IdentityResultAssert.IsSuccess(await manager.CreateAsync(u));
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await manager.AddToRoleAsync(u, "bogus"));
        }

    }
}
