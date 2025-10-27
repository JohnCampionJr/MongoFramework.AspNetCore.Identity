// https://github.com/dotnet/aspnetcore/blob/master/src/Identity/EntityFrameworkCore/src/IdentityEntityFrameworkBuilderExtensions.cs

using System;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoEntityFramework;
using MongoEntityFramework.AspNetCore.Identity;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IdentityBuilder"/> for adding Mongo Framework stores.
    /// </summary>
    public static class MongoIdentityBuilderExtensions
    {
        /// <summary>
        /// Adds an Mongo Framework implementation of identity information stores.
        /// </summary>
        /// <typeparam name="TContext">The Mongo Framework database context to use.</typeparam>
        /// <param name="builder">The <see cref="IdentityBuilder"/> instance this method extends.</param>
        /// <returns>The <see cref="IdentityBuilder"/> instance this method extends.</returns>
        public static IdentityBuilder AddMongoEntityFrameworkStores<TContext>(this IdentityBuilder builder)
            where TContext : DbContext
        {
            AddStores(builder.Services, builder.UserType, builder.RoleType, typeof(TContext));
            return builder;
        }

        private static void AddStores(IServiceCollection services, Type userType, Type roleType, Type contextType)
        {
            var identityUserType = FindGenericBaseType(userType, typeof(MongoIdentityUser<>));
            if (identityUserType == null)
            {
                throw new InvalidOperationException($"{userType.Name} does not inherit from MongoIdentityUser");
            }

            var keyType = identityUserType.GenericTypeArguments[0];

            if (roleType != null)
            {
                var identityRoleType = FindGenericBaseType(roleType, typeof(MongoIdentityRole<>));
                if (identityRoleType == null)
                {
                    throw new InvalidOperationException($"{roleType.Name} does not inherit from MongoIdentityRole");
                }

                Type userStoreType = null;
                Type roleStoreType = null;

                var identityContext = FindGenericBaseType(contextType, typeof(MongoIdentityDbContext<,,,,,,,>));
                if (identityContext == null)
                {
                    // If its a custom DbContext, we can only add the default POCOs
                    userStoreType = typeof(MongoUserStore<,,,>).MakeGenericType(userType, roleType, contextType, keyType);
                    roleStoreType = typeof(MongoRoleStore<,,>).MakeGenericType(roleType, contextType, keyType);
                }
                else
                {
                    userStoreType = typeof(MongoUserStore<,,,,,,,,>).MakeGenericType(userType, roleType, contextType,
                        identityContext.GenericTypeArguments[2],
                        identityContext.GenericTypeArguments[3],
                        identityContext.GenericTypeArguments[4],
                        identityContext.GenericTypeArguments[5],
                        identityContext.GenericTypeArguments[7],
                        identityContext.GenericTypeArguments[6]);
                    roleStoreType = typeof(MongoRoleStore<,,,,>).MakeGenericType(roleType, contextType,
                        identityContext.GenericTypeArguments[2],
                        identityContext.GenericTypeArguments[4],
                        identityContext.GenericTypeArguments[6]);
                }

                services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
                services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), roleStoreType);
            }
            else
            {   // No Roles
                Type userStoreType = null;

                var identityContext = FindGenericBaseType(contextType, typeof(MongoIdentityUserContext<,,,,>));
                if (identityContext == null)
                {
                    // If its a custom DbContext, we can only add the default POCOs
                    userStoreType = typeof(MongoUserOnlyStore<,,>).MakeGenericType(userType, contextType, keyType);
                }
                else
                {
                    userStoreType = typeof(MongoUserOnlyStore<,,,,,>).MakeGenericType(userType, contextType,
                        identityContext.GenericTypeArguments[1],
                        identityContext.GenericTypeArguments[2],
                        identityContext.GenericTypeArguments[3],
                        identityContext.GenericTypeArguments[4]);
                }

                services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
            }

        }

        private static TypeInfo FindGenericBaseType(Type currentType, Type genericBaseType)
        {
            var type = currentType;
            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == genericBaseType)
                {
                    return typeInfo;
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}
