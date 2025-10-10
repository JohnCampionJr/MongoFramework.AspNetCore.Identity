using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MongoEntityFramework.AspNetCore.Identity
{
    public class MongoIdentityRole : MongoIdentityRole<string>
    {
        public MongoIdentityRole() { }

        public MongoIdentityRole(string name) : base(name) { }
    }

    public class MongoIdentityRole<TKey> : IdentityRole<TKey> where TKey : IEquatable<TKey>
    {
        public MongoIdentityRole()
        {
            Claims = new List<IdentityRoleClaim<TKey>>();
        }

        public MongoIdentityRole(string name) : this()
        {
            Check.NotNull(name, nameof(name));
            Name = name;
            NormalizedName = name.Normalize().ToUpperInvariant();
        }

        public override string ToString()
        {
            return Name;
        }

        public List<IdentityRoleClaim<TKey>> Claims { get; set; }
    }
}
