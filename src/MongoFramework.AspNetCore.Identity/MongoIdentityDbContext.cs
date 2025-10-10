using System;
using System.Numerics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;

namespace MongoEntityFramework.AspNetCore.Identity
{
    /// <summary>
    /// Base class for the Mongo Framework database context used for identity.
    /// </summary>
    public class MongoIdentityDbContext : MongoIdentityDbContext<MongoIdentityUser, MongoIdentityRole, string>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MongoIdentityDbContext"/>.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public MongoIdentityDbContext(DbContextOptions options) : base(options) { }
        public MongoIdentityDbContext() : base() { }
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
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public MongoIdentityDbContext(DbContextOptions options) : base(options) { }
        public MongoIdentityDbContext() : base() { }
    }

    /// <summary>
    /// Base class for the Mongo Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of the user objects.</typeparam>
    public class MongoIdentityDbContext<TUser, TRole> : MongoIdentityDbContext<TUser, TRole, string> where TUser : MongoIdentityUser where TRole : MongoIdentityRole
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MongoIdentityDbContext"/>.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public MongoIdentityDbContext(DbContextOptions options) : base(options) { }
        public MongoIdentityDbContext() : base() { }
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
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public MongoIdentityDbContext(DbContextOptions options) : base(options) { }
        public MongoIdentityDbContext() : base() { }
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
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public MongoIdentityDbContext(DbContextOptions options) : base(options) { }
        public MongoIdentityDbContext() : base() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            this.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TUser>().OwnsMany(p => p.Claims,
                b =>
                {
                    b.HasKey("Id"); // Configure "Id" as the primary key
                });
            modelBuilder.Entity<TRole>().OwnsMany(p => p.Claims,
                b =>
                {
                    b.HasKey("Id"); // Configure "Id" as the primary key
                });
            modelBuilder.Entity<TUser>().Property(p => p.Id)
                .HasBsonRepresentation(BsonType.ObjectId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<TRole>().Property(p => p.Id)
                .HasBsonRepresentation(BsonType.ObjectId)
                .ValueGeneratedOnAdd();
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of roles.
        /// </summary>
        public virtual DbSet<TRole> Roles { get; set; }
    }
}
