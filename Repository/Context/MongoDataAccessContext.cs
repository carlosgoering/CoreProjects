
using Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Repository.Context;

public class MongoDataAccessContext<TEntity> : IDataContext<TEntity> where TEntity : IBaseEntity
{
    private readonly IMongoCollection<TEntity> collection;

    public MongoDataAccessContext(IOptions<Database> databaseSettings, string collectionName)
    {
        collection = new MongoClient(databaseSettings.Value.ConnectionString)
            .GetDatabase(databaseSettings.Value.DatabaseName)
            .GetCollection<TEntity>(collectionName);
    }

    public IQueryable<TEntity> Query => collection.AsQueryable();

    public async Task InsertAsync(TEntity entity)
    {
        await collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await collection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq(e => e.id, entity.id), entity);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        await collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq(e => e.id, entity.id));
    }
}
