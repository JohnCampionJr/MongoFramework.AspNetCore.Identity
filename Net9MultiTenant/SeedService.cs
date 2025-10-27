using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Net9MultiTenant.Data;
using Net9MultiTenant.Models;

namespace Net9MultiTenant;

/// <summary>
/// Seed the database the multi-tenant store we'll need.
/// When application has started
/// </summary>
public static class SeedService
{
    public static async Task Seed(WebApplication app)
    {
        var config = app.Services.GetRequiredService<IConfiguration>();
        using var scope = app.Services.CreateScope();

        var store = scope.ServiceProvider.GetRequiredService<IMultiTenantStore<TenantInfo>>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await SetupStore(store);
        await SetupDb(context, store, config, scope.ServiceProvider);
    }

    private static async Task SetupStore(IMultiTenantStore<TenantInfo> store)
    {
        if (store.GetAllAsync().Result.Any()) return;

        await store.TryAddAsync(new TenantInfo { Id = "tenant-acme-d043favoiaw", Identifier = "acme", Name = "ACME Inc" });
        await store.TryAddAsync(new TenantInfo { Id = "tenant-initech-341ojadsfa", Identifier = "initech", Name = "Initech LLC" });
        await store.TryAddAsync(new TenantInfo { Id = "tenant-megacorp-g754dafg", Identifier = "megacorp", Name = "MegaCorp Inc" });
    }

    private static async Task SetupDb(ApplicationDbContext context, IMultiTenantStore<TenantInfo> store, IConfiguration config, IServiceProvider serviceProvider)
    {
        var ti = await store.TryGetByIdentifierAsync("acme");
        
        using var db1 = MultiTenantDbContext.Create<ApplicationDbContext, TenantInfo>(ti, serviceProvider);

        if (!db1.ToDoItems.Any())
        {
            db1.ToDoItems.Add(new ToDoItem { Id = "acme1", Title = "ACME Call Lawyer ", Completed = false });
            db1.ToDoItems.Add(new ToDoItem { Id = "acme2", Title = "File Papers", Completed = false });
            db1.ToDoItems.Add(new ToDoItem { Id = "acme3", Title = "Send Invoices", Completed = true });
            await db1.SaveChangesAsync();
        }

        ti = store.TryGetByIdentifierAsync("megacorp").Result;
        using var db2 = MultiTenantDbContext.Create<ApplicationDbContext, TenantInfo>(ti, serviceProvider);
        if (!db2.ToDoItems.Any())
        {
            db2.ToDoItems.Add(new ToDoItem { Id = "mega1", Title = "MEGA Send Invoices", Completed = true });
            db2.ToDoItems.Add(new ToDoItem { Id = "mega2", Title = "Construct Additional Pylons", Completed = true });
            db2.ToDoItems.Add(new ToDoItem { Id = "mega3", Title = "Call Insurance Company", Completed = false });
            await db2.SaveChangesAsync();
        }

        ti = store.TryGetByIdentifierAsync("initech").Result;
        using var db3 = MultiTenantDbContext.Create<ApplicationDbContext, TenantInfo>(ti, serviceProvider);
        if (!db3.ToDoItems.Any())
        {
            db3.ToDoItems.Add(new ToDoItem { Id = "ini1", Title = "INI Send Invoices", Completed = false });
            db3.ToDoItems.Add(new ToDoItem { Id = "ini2", Title = "Pay Salaries", Completed = true });
            db3.ToDoItems.Add(new ToDoItem { Id = "ini3", Title = "Write Memo", Completed = false });
            await db3.SaveChangesAsync();
        }
    }

}

