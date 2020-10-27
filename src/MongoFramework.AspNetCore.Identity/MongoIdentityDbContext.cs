using System;
using Microsoft.AspNetCore.Identity;

namespace MongoFramework.AspNetCore.Identity
{
    /// <summary>
    /// Base class for the Mongo Framework database context used for identity.
    /// </summary>
    public class MongoIdentityDbContext : MongoIdentityDbContext<MongoIdentityUser, MongoIdentityRole, string>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MongoIdentityDbContext"/>.
        /// </summary>
        /// <param name="connection">The connection to be used by a <see cref="MongoDbContext"/>.</param>
        public MongoIdentityDbContext(IMongoDbConnection connection) : base(connection) { }
    }

    /// <summary>
    /// Base class for the Mongo Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of the user objects.</typeparam>
    public class MongoIdentityDbContext<TUser> : MongoIdentityDbContext<TUser, MongoIdentityRole, string> where TUser : MongoIdentityUser
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MongoIdentityDbContext"/>.
        /// </summary>
        /// <param name="connection">The connection to be used by a <see cref="MongoDbContext"/>.</param>
        public MongoIdentityDbContext(MongoDbConnection connection) : base(connection) { }
    }

    /// <summary>
    /// Base class for the Mongo Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of user objects.</typeparam>
    /// <typeparam name="TRole">The type of role objects.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
    public class MongoIdentityDbContext<TUser, TRole, TKey> : MongoIdentityDbContext<TUser, TRole, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
        where TUser : MongoIdentityUser<TKey>
        where TRole : MongoIdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the db context.
        /// </summary>
        /// <param name="connection">The connection to be used by a <see cref="MongoDbContext"/>.</param>
        public MongoIdentityDbContext(IMongoDbConnection connection) : base(connection) { }
    }

    /// <summary>
    /// Base class for the Mongo Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of user objects.</typeparam>
    /// <typeparam name="TRole">The type of role objects.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
    /// <typeparam name="TUserClaim">The type of the user claim object.</typeparam>
    /// <typeparam name="TUserRole">The type of the user role object.</typeparam>
    /// <typeparam name="TUserLogin">The type of the user login object.</typeparam>
    /// <typeparam name="TRoleClaim">The type of the role claim object.</typeparam>
    /// <typeparam name="TUserToken">The type of the user token object.</typeparam>
    public abstract class MongoIdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> : MongoIdentityUserContext<TUser, TKey, TUserClaim, TUserLogin, TUserToken>
        where TUser : MongoIdentityUser<TKey>
        where TRole : MongoIdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="connection">The connection to be used by a <see cref="MongoDbContext"/>.</param>
        public MongoIdentityDbContext(IMongoDbConnection connection) : base(connection) { }

        /// <summary>
        /// Gets or sets the <see cref="MongoDbSet{TEntity}"/> of roles.
        /// </summary>
        public virtual MongoDbSet<TRole> Roles { get; set; }
    }
}
