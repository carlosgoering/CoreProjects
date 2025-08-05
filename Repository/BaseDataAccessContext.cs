
using Domain.Entities;
using System.Linq.Expressions;

namespace Repository;

public abstract class BaseDataAccessContext<TEntity> where TEntity : class, IBaseEntity, new()
{
    public abstract Task InsertAsync(TEntity entity);

    public abstract Task UpdateAsync(TEntity entity);

    public abstract Task DeleteAsync(TEntity entity);

    public abstract Task<List<TEntity>> SelectAsync();

    public abstract Task<List<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> filter);

    public abstract Task<TEntity> SelectByIdAsync(string id);

    public abstract Task<TEntity> SelectByExternalIdAsync(string id);
}
