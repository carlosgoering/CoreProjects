
using Domain.Entities;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace Repository.Context.Providers.SQLite;

public class DataAccessContext<TEntity> : IDataContext<TEntity> where TEntity : IBaseEntity
{
    private readonly List<TEntity> collection;

    public DataAccessContext(IOptions<Database> databaseSettings, string collectionName)
    {

    }

    public IQueryable<TEntity> Query => collection.AsQueryable();

    public async Task InsertAsync(TEntity entity)
    {
    }

    public async Task UpdateAsync(TEntity entity)
    {
    }

    public async Task DeleteAsync(TEntity entity)
    {
    }

    public async Task<TEntity?> GetByIdAsync(string id)
    {
        return;
    }

    public async Task<List<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> filter)
    {
        return;
    }
}
