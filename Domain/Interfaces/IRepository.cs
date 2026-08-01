using System.Linq.Expressions;
using Domain.Entities.Shared;

namespace Domain.Interfaces;

public interface IRepository<TEntity>
{
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(string id);

    Task<TEntity?> SelectByIdAsync(string id);

    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);

    Task<PagedResult<TEntity>> SelectPagedAsync(Query<TEntity> query);

    Task<long> CountAsync(Expression<Func<TEntity, bool>>? filter = null);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
}
