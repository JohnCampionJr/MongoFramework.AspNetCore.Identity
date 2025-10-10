// Optional Base DbContext Example
// Refer to the solution LICENSE file for more information.

using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoEntityFramework.AspNetCore.Identity;

namespace Finbuckle.MultiTenant.EntityFrameworkCore;

/// <summary>
/// An Identity database context that enforces tenant integrity on multi-tenant entity types.
/// <remarks>
/// All Identity entity types are multi-tenant by default.
/// </remarks>
/// </summary>
public class MultiTenantMongoIdentityDbContext : MultiTenantMongoIdentityDbContext<MongoIdentityUser>
{
    /// <inheritdoc />
    public MultiTenantMongoIdentityDbContext(IMultiTenantContextAccessor multiTenantContextAccessor) : base(multiTenantContextAccessor)
    {
    }

    /// <inheritdoc />
    public MultiTenantMongoIdentityDbContext(IMultiTenantContextAccessor multiTenantContextAccessor, DbContextOptions options) : base(multiTenantContextAccessor, options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MongoIdentityUser>().IsMultiTenant().AdjustUniqueIndexes();
    }
}

/// <summary>
/// An Identity database context that enforces tenant integrity on multi-tenant entity types.
/// <remarks>
/// TUser is not multitenant by default.
/// All other Identity entity types are multitenant by default.
/// </remarks>
/// </summary>
public abstract class MultiTenantMongoIdentityDbContext<TUser> : MultiTenantMongoIdentityDbContext<TUser, MongoIdentityRole, string>
    where TUser : MongoIdentityUser
{
    /// <inheritdoc />
    protected MultiTenantMongoIdentityDbContext(IMultiTenantContextAccessor multiTenantContextAccessor) : base(multiTenantContextAccessor)
    {
    }

    /// <inheritdoc />
    protected MultiTenantMongoIdentityDbContext(IMultiTenantContextAccessor multiTenantContextAccessor, DbContextOptions options) : base(multiTenantContextAccessor, options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MongoIdentityRole>().IsMultiTenant().AdjustUniqueIndexes();
    }
}

/// <summary>
/// An Identity database context that enforces tenant integrity on multi-tenant entity types.
/// <remarks>
/// TUser and TRole are not multitenant by default.
/// All other Identity entity types are multitenant by default.
/// </remarks>
/// </summary>
public abstract class MultiTenantMongoIdentityDbContext<TUser, TRole, TKey> : MultiTenantMongoIdentityDbContext<TUser, TRole, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
    where TUser : MongoIdentityUser<TKey>
    where TRole : MongoIdentityRole<TKey>
    where TKey : IEquatable<TKey>
{
    /// <inheritdoc />
    protected MultiTenantMongoIdentityDbContext(IMultiTenantContextAccessor multiTenantContextAccessor) : base(multiTenantContextAccessor)
    {
    }

    /// <inheritdoc />
    protected MultiTenantMongoIdentityDbContext(IMultiTenantContextAccessor multiTenantContextAccessor, DbContextOptions options) : base(multiTenantContextAccessor, options)
    {
    }

}

/// <summary>
/// An Identity database context that enforces tenant integrity on entity types
/// marked with the MultiTenant annotation or attribute.
/// <remarks>
/// No Identity entity types are multitenant by default.
/// </remarks>
/// </summary>
public abstract class MultiTenantMongoIdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> : MongoIdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>, IMultiTenantDbContext
    where TUser : MongoIdentityUser<TKey>
    where TRole : MongoIdentityRole<TKey>
    where TUserClaim : IdentityUserClaim<TKey>
    where TUserRole : IdentityUserRole<TKey>
    where TUserLogin : IdentityUserLogin<TKey>
    where TRoleClaim : IdentityRoleClaim<TKey>
    where TUserToken : IdentityUserToken<TKey>
    where TKey : IEquatable<TKey>
{
    public ITenantInfo? TenantInfo { get; }

    public TenantMismatchMode TenantMismatchMode { get; set; } = TenantMismatchMode.Throw;

    public TenantNotSetMode TenantNotSetMode { get; set; } = TenantNotSetMode.Throw;

    /// <summary>
    /// Constructs the database context instance and binds to the current tenant.
    /// </summary>
    /// <param name="multiTenantContextAccessor">The MultiTenantContextAccessor instance used to bind the context instance to a tenant.</param>
    protected MultiTenantMongoIdentityDbContext(IMultiTenantContextAccessor multiTenantContextAccessor)
    {
        TenantInfo = multiTenantContextAccessor.MultiTenantContext.TenantInfo;
    }

    /// <summary>
    /// Constructs the database context instance and binds to the current tenant.
    /// </summary>
    /// <param name="multiTenantContextAccessor">The MultiTenantContextAccessor instance used to bind the context instance to a tenant.</param>
    /// <param name="options">The database options instance.</param>
    protected MultiTenantMongoIdentityDbContext(IMultiTenantContextAccessor multiTenantContextAccessor, DbContextOptions options) : base(options)
    {
        TenantInfo = multiTenantContextAccessor.MultiTenantContext.TenantInfo;
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigureMultiTenant();
    }

    /// <inheritdoc />
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.EnforceMultiTenant();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    /// <inheritdoc />
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        this.EnforceMultiTenant();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(false);
    }
}
