using DataAccess.Abstractions.Interfaces;
using DataAccess.Abstractions.Models;

namespace DataAccess.Core.Repository;

public class DataRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IBaseEntity, new()
{
    private readonly IDataAccessContext<TEntity> dataContext;

    public DataRepository(IDataAccessContext<TEntity> dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        await dataContext.InsertAsync(entity);
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        await dataContext.UpdateAsync(entity);
        return entity;
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await dataContext.SelectByIdAsync(id);

        if (entity is null)
            return;

        await dataContext.DeleteAsync(entity);
    }

    public Task<TEntity?> SelectByIdAsync(string id)
        => dataContext.SelectByIdAsync(id);

    public Task<TEntity?> FirstOrDefaultAsync(Query<TEntity> query)
        => dataContext.FirstOrDefaultAsync(query);

    public Task<PagedResult<TEntity>> SelectPagedAsync(
        Query<TEntity> query)
        => dataContext.SelectPagedAsync(query);

    public Task<long> CountAsync(Query<TEntity>? query = null)
        => dataContext.CountAsync(query);

    public Task<bool> ExistsAsync(Query<TEntity> query)
        => dataContext.ExistsAsync(query);
}