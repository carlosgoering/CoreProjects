
using Domain.Entities;
using Domain.Entities.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Repository.MongoDB;

/// <summary>
/// If you want to know more about MongoDB, please visit: https://www.mongodb.com/docs/drivers/csharp/current/usage-examples/#std-label-csharp-usage-examples
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class DataAccessContext<TEntity> : BaseDataAccessContext<TEntity> where TEntity : class, IBaseEntity, new()
{
    private readonly IMongoCollection<TEntity> collection;

    public DataAccessContext(IOptions<Database> databaseSettings, string collectionName)
    {
        collection = new MongoClient(databaseSettings.Value.ConnectionString)
            .GetDatabase(databaseSettings.Value.DatabaseName)
            .GetCollection<TEntity>(collectionName);
    }

    public override async Task InsertAsync(TEntity entity)
    {
        await collection.InsertOneAsync(entity);
    }

    public override async Task UpdateAsync(TEntity entity)
    {
        await collection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq(e => e.id, entity.id), entity);
    }

    public override async Task DeleteAsync(TEntity entity)
    {
        await collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq(e => e.id, entity.id));
    }

    public override async Task<TEntity?> SelectByIdAsync(string id)
    {
        return await collection.Find(Builders<TEntity>.Filter.Eq(e => e.id, id)).FirstOrDefaultAsync();
    }

    public override async Task<List<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await collection.Find(filter).ToListAsync();
    }

    public override async Task<List<TEntity>> SelectAsync()
    {
        return await collection.AsQueryable().ToListAsync();
    }


    public override async Task<TEntity> SelectByExternalIdAsync(string id)
    {
        return await collection.Find(Builders<TEntity>.Filter.Eq(e => e.externalIdentity, id)).FirstOrDefaultAsync();
    }
}
