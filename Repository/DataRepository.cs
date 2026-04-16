using Domain.Entities;
using Domain.Interfaces;
using System.Linq.Expressions;

namespace Repository;

public class DataRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IBaseEntity, new()
{
    private readonly IDataAcessContext<TEntity> dataContext;

    public DataRepository(IDataAcessContext<TEntity> dataContext)
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

    public async Task<List<TEntity>> SelectAsync() => await dataContext.SelectAsync();

    public async Task<TEntity?> SelectAsync(string id) => await dataContext.SelectByIdAsync(id);

    public async Task<bool> DeleteAsync(string id)
    {
        var entity = await SelectAsync(id);
        if (entity == null)
        {
            return false;
        }

        await dataContext.DeleteAsync(entity);
        return true;
    }

    public async Task<List<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> filter) => await dataContext.SelectAsync(filter);
}