using DataAccess.Abstractions.Interfaces;
using DataAccess.Abstractions.Models;
using Microsoft.Extensions.Options;
using SQLite;
using System.Linq.Expressions;

namespace DataAccess.SQLite.Context;

/// <summary>
/// If you want to know more about SQLite, please visit: https://github.com/praeclarum/sqlite-net
/// </summary>
/// <typeparam name="TEntity"></typeparam>
internal class DataAccessContext<TEntity> : IDataAccessContext<TEntity> where TEntity : class, IBaseEntity, new()
{
    private readonly SQLiteAsyncConnection database;

    public DataAccessContext(IOptions<Database> databaseSettings)
    {
        var options = new SQLiteConnectionString(databaseSettings.Value.ConnectionString, true, databaseSettings.Value.ConnectionKey);
        database = new SQLiteAsyncConnection(options);

        database.CreateTableAsync<TEntity>();
    }

    public async Task InsertAsync(TEntity entity) =>  await database.InsertAsync(entity);

    public async Task UpdateAsync(TEntity entity) => await database.UpdateAsync(entity);

    public async Task DeleteAsync(TEntity entity) => await database.DeleteAsync(entity);

    public async Task<TEntity> SelectByIdAsync(string id) => await database.Table<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));

    public async Task<TEntity> SelectByExternalIdAsync(string id) => await database.Table<TEntity>().FirstOrDefaultAsync(x => x.ExternalId.Equals(id));

    public async Task<TEntity?> SelectAsync(string id) => await database.Table<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter) => await database.Table<TEntity>().FirstOrDefaultAsync(filter);

    public async Task<IReadOnlyCollection<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> filter) => await database.Table<TEntity>().Where(filter).ToListAsync();

    public async Task<PagedResult<TEntity>> SelectPagedAsync(Query<TEntity> query)
    {
        var table = database.Table<TEntity>();

        if (query.Filter is not null)
            table = table.Where(query.Filter);

        var total = await table.CountAsync();

        if (query.OrderBy is not null)
        {
            table = query.Descending
                ? table.OrderByDescending(query.OrderBy)
                : table.OrderBy(query.OrderBy);
        }

        var items = await table
            .Skip(query.Skip)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<TEntity>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = total
        };
    }

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>>? filter = null) => await database.Table<TEntity>().Where(filter).CountAsync();

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter) => await database.Table<TEntity>().Where(filter).CountAsync() > 0;
}
