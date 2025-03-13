using Domain.Entities;
using Microsoft.Extensions.Options;
using Repository.Context.Providers.SQLite.Interfaces;

namespace Repository.Context.Providers.SQLite;

/// <summary>
/// Factory for creating instances of <see cref="DataAccessContext{TEntity}"/>.
/// </summary>
public class DataAccessContextFactory : IDataAccessContextFactory
{
    private readonly IOptions<Database> _databaseSettings;

    public DataAccessContextFactory(IOptions<Database> databaseSettings)
    {
        _databaseSettings = databaseSettings;
    }

    public DataAccessContext<TEntity> Create<TEntity>(string tableName) where TEntity : IBaseEntity
    {
        return new DataAccessContext<TEntity>(_databaseSettings, tableName);
    }
}
