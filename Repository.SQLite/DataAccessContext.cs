
using Domain.Entities;
using Domain.Entities.Configuration;
using Microsoft.Extensions.Options;
using SQLite;
using System.Linq.Expressions;

namespace Repository.SQLite;

/// <summary>
/// If you want to know more about SQLite, please visit: https://github.com/praeclarum/sqlite-net
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class DataAccessContext<TEntity> : BaseDataAccessContext<TEntity> where TEntity : class, IBaseEntity, new()
{
    private readonly SQLiteAsyncConnection database;

    public DataAccessContext(IOptions<Database> databaseSettings)
    {
        var options = new SQLiteConnectionString(databaseSettings.Value.ConnectionString, true, databaseSettings.Value.ConnectionKey);
        database = new SQLiteAsyncConnection(options);

        database.CreateTableAsync<TEntity>();
    }

    public override async Task InsertAsync(TEntity entity)
    {
        await database.InsertAsync(entity);
    }

    public override async Task UpdateAsync(TEntity entity)
    {
        await database.UpdateAsync(entity);
    }

    public override async Task DeleteAsync(TEntity entity)
    {
        await database.DeleteAsync(entity);
    }

    public override async Task<List<TEntity>> SelectAsync()
    {
        return await database.Table<TEntity>().ToListAsync();
    }

    public override async Task<List<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await database.Table<TEntity>().Where(filter).ToListAsync();
    }

    public override async Task<TEntity> SelectByIdAsync(string id)
    {
        return await database.Table<TEntity>().Where(x => x.id.Equals(id)).FirstOrDefaultAsync();
    }

    public override async Task<TEntity> SelectByExternalIdAsync(string id)
    {
        return await database.Table<TEntity>().Where(x => x.externalIdentity.Equals(id)).FirstOrDefaultAsync();
    }
}
