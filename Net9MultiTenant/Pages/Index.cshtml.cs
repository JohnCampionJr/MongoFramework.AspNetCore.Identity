using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Net9MultiTenant.Data;
using Net9MultiTenant.Models;

namespace Net9MultiTenant.Pages
{
    public class IndexModel(ILogger<IndexModel> logger, ApplicationDbContext dbContext) : PageModel
    {
        public TenantInfo? TenantInfo { get; private set; }
        public IEnumerable<ToDoItem>? ToDoItems { get; private set; } = new List<ToDoItem>();
        public ToDoItem? OtherTenantToDo { get; private set; }
        public ToDoItem? OtherTenantToDoAsync { get; private set; }

        public async Task OnGet()
        {
            TenantInfo = HttpContext.GetMultiTenantContext<TenantInfo>()?.TenantInfo;
            if (TenantInfo != null)
            {
                ToDoItems = dbContext.ToDoItems.ToList();
                // test data leaking
                OtherTenantToDo = dbContext.ToDoItems.Find("acme1");
                OtherTenantToDoAsync = await dbContext.ToDoItems.FindAsync("acme2");
            }

        }
    }
}
