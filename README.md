# Using the DataAccess Library

The DataAccess library is database-agnostic.

The consuming application is responsible for selecting the database provider and supplying its configuration. The library receives those settings and initializes the requested provider.

---

## appsettings.json

```json
{
  "DBOfChoice": "Mongo",

  "MongoDBDatabase": {
    "ConnectionString": "...",
    "DatabaseName": "ProjectManagement"
  },

  "SQLiteDBDatabase": {
    "ConnectionString": "database.db",
    "ConnectionKey": "",
    "DatabaseName": "ProjectManagement"
  }
}
```

`DBOfChoice` currently supports:

* `Mongo`
* `Sqlite`

---

## Registering the Provider

Register the provider during application startup.

```csharp
builder.Services.AddDbProvider(builder.Configuration);
```

Example implementation:

```csharp
public static IServiceCollection AddDbProvider(
    this IServiceCollection services,
    IConfiguration configuration)
{
    var dbChoice = configuration.GetValue<string>(Definitions.DBOfChoice);

    if (string.IsNullOrWhiteSpace(dbChoice))
        throw new InvalidOperationException(
            $"Configuration '{Definitions.DBOfChoice}' was not found.");

    if (!Enum.TryParse<Definitions.DataBases>(dbChoice, true, out var selectedDb))
        throw new InvalidOperationException(
            $"Unsupported database provider: {dbChoice}");

    var sectionName = selectedDb switch
    {
        Definitions.DataBases.Mongo => Definitions.MongoDbDatabaseSection,
        Definitions.DataBases.Sqlite => Definitions.SQLiteDbDatabaseSection,
        _ => throw new InvalidOperationException(
            $"Unsupported database provider: {selectedDb}")
    };

    var section = configuration.GetSection(sectionName);

    if (!section.Exists())
        throw new InvalidOperationException(
            $"Configuration section '{sectionName}' was not found.");

    Action<Database> configure = options => section.Bind(options);

    switch (selectedDb)
    {
        case Definitions.DataBases.Mongo:
            services.AddMongo(configure);
            break;

        case Definitions.DataBases.Sqlite:
            services.AddSqlite(configure);
            break;
    }

    return services;
}
```

`AddDbProvider()` is responsible for:

* Reading the selected provider (`DBOfChoice`).
* Validating that the provider is supported.
* Validating that the corresponding configuration section exists.
* Binding the provider configuration.
* Initializing the selected provider.

Each provider registers all required dependencies internally, including:

* `IDataAccessContext<TEntity>`
* `IRepository<TEntity>`
* Provider-specific services (MongoDB or SQLite)

The application does not need to register repositories, contexts, or provider services manually.

---

## Using the Repository

After registering the provider, repositories can be injected normally.

```csharp
public class ProjectService : IProjectService
{
    private readonly IRepository<Project> repository;

    public ProjectService(IRepository<Project> repository)
    {
        this.repository = repository;
    }
}
```

---

## Switching Providers

Changing the database provider only requires updating the application configuration.

### MongoDB

```json
{
  "DBOfChoice": "Mongo"
}
```

### SQLite

```json
{
  "DBOfChoice": "Sqlite"
}
```

No application code changes are required.

---

## Supported Providers

Currently supported providers:

* MongoDB
* SQLite

Additional providers can be added by implementing a new provider registration extension following the same pattern as `AddMongo(...)` and `AddSqlite(...)`.
