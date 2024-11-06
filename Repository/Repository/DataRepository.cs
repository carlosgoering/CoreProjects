using Domain.Entities;
using System.Linq.Expressions;

namespace Repository;

public class DataRepository<TEntity> where TEntity : IBaseEntity
{
    private readonly IDataContext<TEntity> dataContext;

    public DataRepository(IDataContext<TEntity> dataContext)
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

    public async Task<List<TEntity>> SelectAsync()
    {
        return dataContext.Query.ToList();
    }

    public async Task<TEntity?> SelectAsync(string id)
    {
        return dataContext.Query.FirstOrDefault(e => e.id == id);
    }

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

    public async Task<List<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> filter)
    {
        return dataContext.Query.Where(filter).ToList();
    }
}