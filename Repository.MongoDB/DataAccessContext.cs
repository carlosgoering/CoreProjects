
using Domain.Entities;
using Domain.Entities.Configuration;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Repository.MongoDB;

/// <summary>
/// If you want to know more about MongoDB, please visit: https://www.mongodb.com/docs/drivers/csharp/current/usage-examples/#std-label-csharp-usage-examples
/// </summary>
/// <typeparam name="TEntity"></typeparam>
internal class DataAccessContext<TEntity> : IDataAcessContext<TEntity> where TEntity : class, IBaseEntity, new()
{
    private readonly IMongoCollection<TEntity> collection;

    public DataAccessContext(IOptions<Database> databaseSettings, string collectionName)
    {
        collection = new MongoClient(databaseSettings.Value.ConnectionString)
            .GetDatabase(databaseSettings.Value.DatabaseName)
            .GetCollection<TEntity>(collectionName);
    }

    public async Task InsertAsync(TEntity entity) => await collection.InsertOneAsync(entity);

    public async Task UpdateAsync(TEntity entity) => await collection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id), entity);

    public async Task DeleteAsync(TEntity entity) => await collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id));

    public async Task<TEntity> SelectByIdAsync(string id) => await collection.Find(Builders<TEntity>.Filter.Eq(e => e.Id, id)).FirstOrDefaultAsync();

    public async Task<List<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> filter) => await collection.Find(filter).ToListAsync();

    public async Task<List<TEntity>> SelectAsync() => await collection.AsQueryable().ToListAsync();

    public async Task<TEntity> SelectByExternalIdAsync(string id) => await collection.Find(Builders<TEntity>.Filter.Eq(e => e.ExternalId, id)).FirstOrDefaultAsync();
 
}
