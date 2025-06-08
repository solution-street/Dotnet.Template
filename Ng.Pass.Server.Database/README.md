# EWA Coordination API

## Create a local database
Prerequisites: 
1. Install [SQL Server Developer Free Specialized Edition](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
2. Go through installation Wizard, then click Customize
3. Select Install New Installation
4. Give an instance name that you will later use for your local connection string
5. Add Windows authorization and SQL Server authentication
6. When installation is done, Install [SSMS](https://learn.microsoft.com/en-us/ssms/download-sql-server-management-studio-ssms)
7. Add your local instance to SSMS
8. Create a database named `Coordination` in SSMS

Add connection string to local secrets:

1. Add connection string to `secrets.json` using Visual Studio.
   - Right click on startup project > Manage User Secrets > replace `***` with your local values and `MSSQLSERVER22` with your instance name
```
{
  "ConnectionStrings": {
    "Coordination": "Server=localhost\\MSSQLSERVER22;Database=Coordination;Trusted_Connection=True;TrustServerCertificate=True;"
  },
}
```

## Migrate a local database
Prerequisites: [Create a local database](#Create-a-local-database)

Install the .NET EF Tool:
`dotnet tool install --global dotnet-ef --version 8.*`

.NET Core CLI: 
`dotnet ef database update --project "Ng.Pass.Server.Database" --connection "Server=localhost\MSSQLSERVER22;Database=Coordination;User ID=YourUsername;Password=YourPassword;TrustServerCertificate=True;"`

Package Manager Console: 
`Update-Database -Project "Ng.Pass.Server.Database" -Connection "Server=localhost\MSSQLSERVER22;Database=Coordination;User ID=YourUsername;Password=YourPassword;TrustServerCertificate=True;"`

## Add a column
1. Update the model code to add your changes with data annotations included
2. Add migration through CLI or the Package Manager Console
   - .NET Core CLI: `dotnet ef migrations add AddStuff --project "Ng.Pass.Server.Database"`
   - Package Manager Console: `Add-Migration AddStuff -Project "Ng.Pass.Server.Database"`
3. Apply migration
   - .NET Core CLI: `dotnet ef database update --project "Ng.Pass.Server.Database" --connection "Server=localhost\MSSQLSERVER22;Database=Coordination;User ID=YourUsername;Password=YourPassword;TrustServerCertificate=True;"`
   - Package Manager Console: `Update-Database -Project "Ng.Pass.Server.Database" -Connection "Server=localhost\MSSQLSERVER22;Database=Coordination;User ID=YourUsername;Password=YourPassword;TrustServerCertificate=True;"`
4. Model and database will be synced

### Example
Let's assume the fcc application model looks like this:
```
public class FccApplication
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
}
```

... and you want to add a creation timestamp to your fcc applications. You've done the necessary changes to your application, and your model now looks like this:
```
public class FccApplication
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public DateTime CreatedTimestamp { get; set; }
}
```

Your model and your production database are now out of sync - we must add a new column to your database schema. Let's create a new migration for this:

.NET Core CLI: `dotnet ef migrations add AddFccApplicationCreatedTimestamp --project "Ng.Pass.Server.Database"`

Package Manager Console: `Add-Migration AddFccApplicationCreatedTimestamp -Project "Ng.Pass.Server.Database"`

## Add an index
```
[Index(nameof(Url))]
public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
}
```

### Composite index
```
[Index(nameof(FirstName), nameof(LastName))]
public class Person
{
    public int PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```
