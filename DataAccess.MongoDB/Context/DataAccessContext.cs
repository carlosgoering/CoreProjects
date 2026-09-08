using DataAccess.Abstractions.Interfaces;
using DataAccess.Abstractions.Models;
using DataAccess.MongoDB.ClassMaps;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace DataAccess.MongoDB.Context;

/// <summary>
/// If you want to know more about MongoDB, please visit: https://www.mongodb.com/docs/drivers/csharp/current/usage-examples/#std-label-csharp-usage-examples
/// </summary>
/// <typeparam name="TEntity"></typeparam>
internal sealed class DataAccessContext<TEntity> : IDataAccessContext<TEntity>
    where TEntity : class, IBaseEntity, new()
{
    private readonly IMongoCollection<TEntity> collection;
    public DataAccessContext(IMongoDatabase database)
    {
        ClassMapRegistration.Register<TEntity>();

        collection = database.GetCollection<TEntity>(
            typeof(TEntity).Name);
    }
    public async Task InsertAsync(TEntity entity)
    {
        await collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await collection.ReplaceOneAsync(
            Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id),
            entity);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        await collection.DeleteOneAsync(
            Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id));
    }

    public async Task<TEntity?> SelectByIdAsync(string id)
    {
        return await collection
            .Find(Builders<TEntity>.Filter.Eq(x => x.Id, id))
            .FirstOrDefaultAsync();
    }

    public async Task<TEntity?> FirstOrDefaultAsync(
        Query<TEntity> query)
    {
        var find = BuildQuery(query);

        return await find
            .Limit(1)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<TEntity>> SelectAsync(
        Query<TEntity> query)
    {
        var find = BuildQuery(query);

        return await find
            .Skip(query.Skip)
            .Limit(query.PageSize)
            .ToListAsync();
    }

    public async Task<PagedResult<TEntity>> SelectPagedAsync(
        Query<TEntity> query)
    {
        var find = BuildQuery(query);

        var total = await find.CountDocumentsAsync();

        var items = await find
            .Skip(query.Skip)
            .Limit(query.PageSize)
            .ToListAsync();

        return new PagedResult<TEntity>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = checked((int)total)
        };
    }

    public async Task<long> CountAsync(
        Query<TEntity>? query = null)
    {
        var find = query is null
            ? collection.Find(Builders<TEntity>.Filter.Empty)
            : BuildQuery(query);

        return await find.CountDocumentsAsync();
    }

    public async Task<bool> ExistsAsync(Query<TEntity> query)
    {
        var find = BuildQuery(query);

        return await find
            .Limit(1)
            .AnyAsync();
    }

    private IFindFluent<TEntity, TEntity> BuildQuery(
        Query<TEntity> query)
    {
        var filter = BuildFilter(query);

        var find = collection.Find(filter);

        if (query.Order is not null)
        {
            find = query.Order.Descending
                ? find.SortByDescending(x => query.Order.Field)
                : find.SortBy(x => query.Order.Field);
        }

        return find;
    }

    private FilterDefinition<TEntity> BuildFilter(
        Query<TEntity> query)
    {
        if (query.Filters.Count == 0)
            return Builders<TEntity>.Filter.Empty;

        var filters = query.Filters
            .Select(BuildFilter)
            .ToList();

        return Builders<TEntity>.Filter.And(filters);
    }

    private FilterDefinition<TEntity> BuildFilter(
        QueryFilter filter)
    {
        var builder = Builders<TEntity>.Filter;

        return filter.Operator switch
        {
            QueryOperator.Equal =>
                builder.Eq(filter.Field, filter.Value),

            QueryOperator.NotEqual =>
                builder.Ne(filter.Field, filter.Value),

            QueryOperator.GreaterThan =>
                builder.Gt(filter.Field, filter.Value),

            QueryOperator.GreaterThanOrEqual =>
                builder.Gte(filter.Field, filter.Value),

            QueryOperator.LessThan =>
                builder.Lt(filter.Field, filter.Value),

            QueryOperator.LessThanOrEqual =>
                builder.Lte(filter.Field, filter.Value),

            _ => throw new NotSupportedException(
                $"Operator '{filter.Operator}' is not supported by MongoDB.")
        };
    }
}