
using Domain.Entities;
using Domain.Entities.Configuration;
using Domain.Entities.Shared;
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

    private readonly IMongoCollection<TEntity> collection;

    public DataAccessContext(IMongoDatabase database, string collectionName)
    {
        collection = database.GetCollection<TEntity>(collectionName);
    }

    public async Task InsertAsync(TEntity entity) => await collection.InsertOneAsync(entity);

    public async Task UpdateAsync(TEntity entity) => await collection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id), entity);

    public async Task DeleteAsync(TEntity entity) => await collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id));

    public async Task<TEntity> SelectByIdAsync(string id) => await collection.Find(Builders<TEntity>.Filter.Eq(e => e.Id, id)).FirstOrDefaultAsync();

    public async Task<TEntity> SelectByExternalIdAsync(string id) => await collection.Find(Builders<TEntity>.Filter.Eq(e => e.ExternalId, id)).FirstOrDefaultAsync();

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter) => await collection.Find(filter).FirstOrDefaultAsync();

    public async Task<IReadOnlyCollection<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> filter) => await collection.Find(filter).ToListAsync();

    public async Task<PagedResult<TEntity>> SelectPagedAsync(Query<TEntity> query)
    {
        var find = query.Filter is null
            ? collection.Find(Builders<TEntity>.Filter.Empty)
            : collection.Find(query.Filter);

        var total = (int)await find.CountDocumentsAsync();

        if (query.OrderBy is not null)
        {
            find = query.Descending
                ? find.SortByDescending(query.OrderBy)
                : find.SortBy(query.OrderBy);
        }

        var items = await find
            .Skip(query.Skip)
            .Limit(query.PageSize)
            .ToListAsync();

        return new PagedResult<TEntity>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = total
        };
    }

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>>? filter = null) => await collection.CountDocumentsAsync(filter);

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter) => await collection.Find(filter).AnyAsync();
}
