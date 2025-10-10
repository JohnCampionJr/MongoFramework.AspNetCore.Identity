using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MongoEntityFramework.AspNetCore.Identity
{
    public class MongoIdentityUser : MongoIdentityUser<string>
    {
        public MongoIdentityUser() { }

        public MongoIdentityUser(string userName) : base(userName) { }
    }

    public class MongoIdentityUser<TKey> : IdentityUser<TKey> where TKey : IEquatable<TKey>
    {
        public MongoIdentityUser()
        {
            Roles = new List<TKey>();
            Claims = new List<IdentityUserClaim<TKey>>();
            Logins = new List<IdentityUserLogin<TKey>>();
            Tokens = new List<IdentityUserToken<TKey>>();
        }

        public MongoIdentityUser(string userName) : this()
        {
            Check.NotNull(userName, nameof(userName));
            UserName = userName;
            NormalizedUserName = userName.Normalize().ToUpperInvariant();
        }

        public List<TKey> Roles { get; set; }

        public List<IdentityUserClaim<TKey>> Claims { get; set; }

        public List<IdentityUserLogin<TKey>> Logins { get; set; }

        public List<IdentityUserToken<TKey>> Tokens { get; set; }
    }
}
