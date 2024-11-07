using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IService<TEntity>
{
    bool Create(TEntity item);
    void Delete(string id);
    List<TEntity> Get();
    TEntity Get(string id);
    List<TEntity> Get(Expression<Func<TEntity, bool>> filter);
    bool Update<BaseValidator>(TEntity login);
}
