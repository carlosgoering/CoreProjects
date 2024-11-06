using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IRepository<TEntity>
{
    TEntity Insert(TEntity entity);
    TEntity Update(TEntity entity);
    bool Delete(string id);
    List<TEntity> Select();
    TEntity? Select(string id);
    List<TEntity>? Select(Expression<Func<TEntity, bool>> filter);
}
