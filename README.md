![alt text](https://raw.githubusercontent.com/jcamp-code/FluentEmail/main/assets/mongoidentity_logo_64x64.png "Mongo Identity")

# MongoEntityFramework.AspNetCore.Identity
Asp.Net Core Identity providers for [MongoDB.EntityFrameworkCore](https://github.com/mongodb/mongo-efcore-provider).

## Features
MongoFramework Implementations
- IdentityUser
- IdentityRole
- RoleStore
- UserStore
- UserOnlyStore

ServiceCollection Extensions for
- MongoDbContext
````cs
services.AddMongoDbContext<MongoDbContext>(o =>
    o.ConnectionString = Configuration.GetConnectionString("DefaultConnection"));
````

- Identity Stores (adds to IdentityBuilder)
````cs
services.AddDefaultIdentity<MongoIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddMongoFrameworkStores<MongoDbContext>();
````

- Complete Identity (User Only)
````cs
services.AddMongoDbContext<MongoDbContext>(o =>
    o.ConnectionString = Configuration.GetConnectionString("DefaultConnection"));

services.AddDefaultMongoIdentity<MongoIdentityUser, MongoDbContext>();
````

- Complete Identity (Users and Roles)
````cs
services.AddMongoDbContext<MongoDbContext>(o =>
    o.ConnectionString = Configuration.GetConnectionString("DefaultConnection"));

services.AddMongoIdentity<MongoIdentityUser, MongoIdentityRole, MongoDbContext>();
````

Sample .NET Core Project

Unit Tests, including passing Asp.Net Core's IdentitySpecificationBase

## IdentitySpec Tests
[This issue](https://github.com/dotnet/aspnetcore/issues/27873) shows the spec tests weren't
publicly released for .NET 5.0.  They are supposed to be, but do not show up on NuGet yet.
I have added the code manually to the test project until this gets published.

