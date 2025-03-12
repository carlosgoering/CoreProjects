
using Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Repository.Context.Providers;

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

    public async Task<TEntity?> GetByIdAsync(string id)
    {
        return await collection.Find(Builders<TEntity>.Filter.Eq(e => e.id, id)).FirstOrDefaultAsync();
    }

    public async Task<List<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await collection.Find(filter).ToListAsync();
    }
}
