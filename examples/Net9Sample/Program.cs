using Microsoft.EntityFrameworkCore;
using MongoEntityFramework.AspNetCore.Identity;
using Net9Sample.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMongoDB(connectionString);
});

//builder.Services.AddDefaultMongoIdentity<MongoIdentityUser, ApplicationDbContext>(options => options.SignIn.RequireConfirmedAccount = true);    
builder.Services.AddDefaultIdentity<MongoIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddMongoEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
