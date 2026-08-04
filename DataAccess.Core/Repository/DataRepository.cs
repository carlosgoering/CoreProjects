using DataAccess.Abstractions.Interfaces;
using DataAccess.Abstractions.Models;
using System.Linq.Expressions;

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

    public async Task<TEntity?> SelectAsync(string id) => await dataContext.SelectByIdAsync(id);

    public async Task DeleteAsync(string id) => await dataContext.DeleteAsync(new TEntity { Id = id });

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter) => await dataContext.FirstOrDefaultAsync(filter);

    public async Task<PagedResult<TEntity>> SelectPagedAsync(Query<TEntity> query) => await dataContext.SelectPagedAsync(query);

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>>? filter = null) => await dataContext.CountAsync(filter);

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter) => await dataContext.ExistsAsync(filter);

    public async Task<TEntity?> SelectByIdAsync(string id) => await dataContext.SelectByIdAsync(id);
}