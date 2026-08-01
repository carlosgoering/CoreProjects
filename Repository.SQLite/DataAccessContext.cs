
using Domain.Entities;
using Domain.Entities.Configuration;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using SQLite;
using System.Linq.Expressions;

namespace Repository.SQLite;

/// <summary>
/// If you want to know more about SQLite, please visit: https://github.com/praeclarum/sqlite-net
/// </summary>
/// <typeparam name="TEntity"></typeparam>
internal class DataAccessContext<TEntity> : IDataAcessContext<TEntity> where TEntity : class, IBaseEntity, new()
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

    public async Task<List<TEntity>> SelectAsync() => await database.Table<TEntity>().ToListAsync();

    public async Task<List<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> filter) => await database.Table<TEntity>().Where(filter).ToListAsync();

    public async Task<TEntity> SelectByIdAsync(string id) => await database.Table<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));

    public async Task<TEntity> SelectByExternalIdAsync(string id) => await database.Table<TEntity>().FirstOrDefaultAsync(x => x.ExternalId.Equals(id));
}
