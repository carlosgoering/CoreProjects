using Domain.Entities;
using Microsoft.Extensions.Options;
using Repository.Context.Interfaces;

namespace Repository.Context;

public class MongoDataAccessContextFactory : IMongoDataAccessContextFactory
{
    private readonly IOptions<Database> _databaseSettings;

    public MongoDataAccessContextFactory(IOptions<Database> databaseSettings)
    {
        _databaseSettings = databaseSettings;
    }

    public MongoDataAccessContext<TEntity> Create<TEntity>(string collectionName) where TEntity : IBaseEntity
    {
        return new MongoDataAccessContext<TEntity>(_databaseSettings, collectionName);
    }
}
