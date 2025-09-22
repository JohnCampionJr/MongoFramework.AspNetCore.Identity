using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoFramework;
using MongoFramework.AspNetCore.Identity;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class MongoFrameworkServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDbContext<TContext>(
            this IServiceCollection serviceCollection,
            Action<MongoDbContextOptions> contextOptionsAction = null,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime connectionLifetime = ServiceLifetime.Scoped)
            where TContext : DbContext
            => AddMongoDbContext<TContext, TContext>(serviceCollection, contextOptionsAction, contextLifetime, connectionLifetime);

        public static IServiceCollection AddMongoDbContext<TContextService, TContextImplementation>(
            this IServiceCollection serviceCollection,
            Action<MongoDbContextOptions> contextOptionsAction = null,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime connectionLifetime = ServiceLifetime.Scoped)
            where TContextImplementation : DbContext, TContextService
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));

            var contextOptions = new MongoDbContextOptions();
            contextOptionsAction?.Invoke(contextOptions);

            if (!string.IsNullOrEmpty(contextOptions.ConnectionString))
            {
                var mainContextMongoUri = new MongoDB.Driver.MongoUrl(contextOptions.ConnectionString);

                var d = new ServiceDescriptor(typeof(IMongoDbConnection),
                    s =>
                    {
                        var connection = MongoDbConnection.FromUrl(mainContextMongoUri);
                        if (contextOptions.DiagnosticListener != null)
                        {
                            connection.DiagnosticListener = contextOptions.DiagnosticListener;
                        }
                        return connection;
                    },
                    connectionLifetime
                );

                serviceCollection.TryAdd(d);
            }

            serviceCollection.TryAdd(new ServiceDescriptor(typeof(TContextService), typeof(TContextImplementation), contextLifetime));

            return serviceCollection;
        }

    }
}
