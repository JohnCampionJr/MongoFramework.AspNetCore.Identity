using System;
using Microsoft.AspNetCore.Identity;

namespace MongoFramework.AspNetCore.Identity
{
    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of the user objects.</typeparam>
    public class MongoIdentityUserContext<TUser> : MongoIdentityUserContext<TUser, string> where TUser : MongoIdentityUser
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MongoIdentityUserContext{TUser}"/>.
        /// </summary>
        /// <param name="connection">The connection to be used by a <see cref="MongoDbContext"/>.</param>
        public MongoIdentityUserContext(MongoDbConnection connection) : base(connection) { }

    }

    /// <summary>
    /// Base class for the Mongo Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of user objects.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
    public class MongoIdentityUserContext<TUser, TKey> : MongoIdentityUserContext<TUser, TKey, IdentityUserClaim<TKey>,
        IdentityUserLogin<TKey>, IdentityUserToken<TKey>>
        where TUser : MongoIdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the db context.
        /// </summary>
        /// <param name="connection">The connection to be used by a <see cref="MongoDbContext"/>.</param>
        public MongoIdentityUserContext(MongoDbConnection connection) : base(connection) { }

    }

    /// <summary>
    /// Base class for the Mongo Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of user objects.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
    /// <typeparam name="TUserClaim">The type of the user claim object.</typeparam>
    /// <typeparam name="TUserLogin">The type of the user login object.</typeparam>
    /// <typeparam name="TUserToken">The type of the user token object.</typeparam>
    public abstract class MongoIdentityUserContext<TUser, TKey, TUserClaim, TUserLogin, TUserToken> : MongoDbContext
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserToken : IdentityUserToken<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="connection">The connection to be used by a <see cref="MongoDbContext"/>.</param>
        public MongoIdentityUserContext(MongoDbConnection connection) : base(connection) { }

        /// <summary>
        /// Gets or sets the <see cref="MongoDbSet{TEntity}"/> of Users.
        /// </summary>
        public virtual MongoDbSet<TUser> Users { get; set; }

    }
}
