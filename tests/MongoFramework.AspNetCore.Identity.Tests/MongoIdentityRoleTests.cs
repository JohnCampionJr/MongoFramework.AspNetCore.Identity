using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Xunit;

namespace MongoEntityFramework.AspNetCore.Identity.Tests
{
    public class MongoIdentityRoleTests
    {
        [Fact]
        public void ConstructorCreatesEmptyLists()
        {
            var role = new MongoIdentityRole();

            role.Claims.ShouldNotBeNull();
            role.Claims.Count.ShouldBe(0);
        }

        [Fact]
        public void ConstructorSucceedsWithValidUserName()
        {
            var role = new MongoIdentityRole("role-name");

            role.Name.ShouldBe("role-name");
            role.NormalizedName.ShouldBe("ROLE-NAME");
            role.ToString().ShouldBe("role-name");
        }

        [Fact]
        public void ConstructorThrowsExceptionWithNullUserName()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                _ = new MongoIdentityRole(null);
            });
        }

        [Fact]
        public void KeyTypeIsUsedInCollectionsWithInteger()
        {
            var role = new MongoIdentityRole<int>();

            role.Claims.ShouldBeOfType<List<IdentityRoleClaim<int>>>();
        }

        [Fact]
        public void KeyTypeIsUsedInCollectionsWithGuid()
        {
            var role = new MongoIdentityRole<Guid>();

            role.Claims.ShouldBeOfType<List<IdentityRoleClaim<Guid>>>();
        }

    }
}
