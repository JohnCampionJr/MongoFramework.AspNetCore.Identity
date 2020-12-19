using System;
using Microsoft.AspNetCore.Identity;
using MongoFramework;
using MongoFramework.AspNetCore.Identity;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class MongoIdentityServiceCollectionExtensions
    {
        public static IdentityBuilder AddDefaultMongoIdentity<TUser>(this IServiceCollection services) where TUser : MongoIdentityUser
            => AddDefaultMongoIdentity<TUser, MongoDbContext>(services, _ => { });

        public static IdentityBuilder AddDefaultMongoIdentity<TUser>(this IServiceCollection services, Action<IdentityOptions> configureOptions) where TUser : MongoIdentityUser
            => AddDefaultMongoIdentity<TUser, MongoDbContext>(services, configureOptions);

        public static IdentityBuilder AddDefaultMongoIdentity<TUser, TContext>(this IServiceCollection services) where TUser : MongoIdentityUser where TContext : MongoDbContext
            => AddDefaultMongoIdentity<TUser, TContext>(services, _ => { });

        public static IdentityBuilder AddDefaultMongoIdentity<TUser, TContext>(this IServiceCollection services, Action<IdentityOptions> configureOptions) where TUser : MongoIdentityUser where TContext : MongoDbContext
        {
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies(o => { });

            return services.AddIdentityCore<TUser>(o =>
            {
                configureOptions?.Invoke(o);
            })
            .AddSignInManager()
            .AddDefaultTokenProviders()
            .AddMongoFrameworkStores<TContext>();
        }

        public static IdentityBuilder AddMongoIdentity<TUser, TRole>(this IServiceCollection services) where TUser : MongoIdentityUser where TRole : MongoIdentityRole
            => AddMongoIdentity<TUser, TRole, MongoDbContext>(services, _ => { });

        public static IdentityBuilder AddMongoIdentity<TUser, TRole>(this IServiceCollection services, Action<IdentityOptions> configureOptions) where TUser : MongoIdentityUser where TRole : MongoIdentityRole
            => AddMongoIdentity<TUser, TRole, MongoDbContext>(services, configureOptions);

        public static IdentityBuilder AddMongoIdentity<TUser, TRole, TContext>(this IServiceCollection services) where TUser : MongoIdentityUser where TContext : MongoDbContext where TRole : MongoIdentityRole
            => AddMongoIdentity<TUser, TRole, TContext>(services, _ => { });

        public static IdentityBuilder AddMongoIdentity<TUser, TRole, TContext>(this IServiceCollection services, Action<IdentityOptions> configureOptions) where TUser : MongoIdentityUser where TContext : MongoDbContext where TRole : MongoIdentityRole
        {
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies(o => { });

            return services.AddIdentityCore<TUser>(o =>
            {
                configureOptions?.Invoke(o);
            })
            .AddSignInManager()
            .AddDefaultTokenProviders()
            .AddRoles<TRole>()
            .AddMongoFrameworkStores<TContext>();
        }
    }
}
