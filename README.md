[![Actions Status](https://github.com/JohnCampionJr/MongoFramework.AspNetCore.Identity/workflows/.NET%20Core%20Coverage%20(Ubuntu)/badge.svg)](https://github.com/JohnCampionJr/MongoFramework.AspNetCore.Identity/actions)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/0fcc6ef3ac7c467593bd001fa79ef1c4)](https://www.codacy.com/gh/JohnCampionJr/MongoFramework.AspNetCore.Identity/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=JohnCampionJr/MongoFramework.AspNetCore.Identity&amp;utm_campaign=Badge_Grade)
[![codecov](https://codecov.io/gh/JohnCampionJr/MongoFramework.AspNetCore.Identity/branch/main/graph/badge.svg?token=9573STFAXG)](undefined)

# MongoFramework.AspNetCore.Identity
.Net Core Identity providers for [MongoFramework](https://github.com/TurnerSoftware/MongoFramework).

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

