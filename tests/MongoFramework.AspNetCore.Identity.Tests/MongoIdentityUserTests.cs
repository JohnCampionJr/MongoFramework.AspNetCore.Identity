using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Xunit;

namespace MongoEntityFramework.AspNetCore.Identity.Tests
{
    public class MongoIdentityUserTests
    {
        [Fact]
        public void ConstructorCreatesEmptyLists()
        {
            var user = new MongoIdentityUser();

            user.Claims.ShouldNotBeNull();
            user.Roles.ShouldNotBeNull();
            user.Tokens.ShouldNotBeNull();
            user.Logins.ShouldNotBeNull();

            user.Claims.Count.ShouldBe(0);
            user.Roles.Count.ShouldBe(0);
            user.Tokens.Count.ShouldBe(0);
            user.Logins.Count.ShouldBe(0);
        }

        [Fact]
        public void ConstructorSucceedsWithValidUserName()
        {
            var user = new MongoIdentityUser("username");

            user.UserName.ShouldBe("username");
            user.NormalizedUserName.ShouldBe("USERNAME");
        }

        [Fact]
        public void ConstructorThrowsExceptionWithNullUserName()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                _ = new MongoIdentityUser(null);
            });
        }

        [Fact]
        public void KeyTypeIsUsedInCollectionsWithInteger()
        {
            var user = new MongoIdentityUser<int>();

            user.Claims.ShouldBeOfType<List<IdentityUserClaim<int>>>();
            user.Tokens.ShouldBeOfType<List<IdentityUserToken<int>>>();
            user.Logins.ShouldBeOfType<List<IdentityUserLogin<int>>>();
        }

        [Fact]
        public void KeyTypeIsUsedInCollectionsWithGuid()
        {
            var user = new MongoIdentityUser<Guid>();

            user.Claims.ShouldBeOfType<List<IdentityUserClaim<Guid>>>();
            user.Tokens.ShouldBeOfType<List<IdentityUserToken<Guid>>>();
            user.Logins.ShouldBeOfType<List<IdentityUserLogin<Guid>>>();
        }



    }
}
