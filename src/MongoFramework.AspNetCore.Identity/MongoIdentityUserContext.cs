using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace MongoEntityFramework.AspNetCore.Identity
{
    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of the user objects.</typeparam>
    public class MongoIdentityUserContext : MongoIdentityUserContext<MongoIdentityUser, string>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MongoIdentityUserContext{TUser}"/>.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public MongoIdentityUserContext(DbContextOptions options) : base(options) { }
        public MongoIdentityUserContext() : base() { }

    }

    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of the user objects.</typeparam>
    public class MongoIdentityUserContext<TUser> : MongoIdentityUserContext<TUser, string> where TUser : MongoIdentityUser
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MongoIdentityUserContext{TUser}"/>.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public MongoIdentityUserContext(DbContextOptions options) : base(options) { }

        public MongoIdentityUserContext() : base() { }

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
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public MongoIdentityUserContext(DbContextOptions options) : base(options) { }
        public MongoIdentityUserContext() : base() { }

    }

    /// <summary>
    /// Base class for the Mongo Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of user objects.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
    /// <typeparam name="TUserClaim">The type of the user claim object.</typeparam>
    /// <typeparam name="TUserLogin">The type of the user login object.</typeparam>
    /// <typeparam name="TUserToken">The type of the user token object.</typeparam>
    public abstract class MongoIdentityUserContext<TUser, TKey, TUserClaim, TUserLogin, TUserToken> : DbContext
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserToken : IdentityUserToken<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public MongoIdentityUserContext(DbContextOptions options) : base(options) { }
        public MongoIdentityUserContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            this.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TUser>().Property(p => p.Id)
                .HasBsonRepresentation(BsonType.ObjectId)
                .ValueGeneratedOnAdd();
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Users.
        /// </summary>
        public virtual DbSet<TUser> Users { get; set; }

    }
}
