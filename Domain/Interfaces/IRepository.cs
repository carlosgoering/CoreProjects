using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IRepository<TEntity>
{
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(string id);
    Task<List<TEntity>> SelectAsync();
    Task<TEntity?> SelectAsync(string id);
    Task<List<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> filter);
}
