using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IDataAcessContext<TEntity> where TEntity : class, IBaseEntity
    {
        public abstract Task InsertAsync(TEntity entity);

        public abstract Task UpdateAsync(TEntity entity);

        public abstract Task DeleteAsync(TEntity entity);

        public abstract Task<List<TEntity>> SelectAsync();

        public abstract Task<List<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> filter);

        public abstract Task<TEntity> SelectByIdAsync(string id);

        public abstract Task<TEntity> SelectByExternalIdAsync(string id);
    }
}
