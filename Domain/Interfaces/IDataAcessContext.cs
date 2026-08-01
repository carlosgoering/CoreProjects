using Domain.Entities;
using Domain.Entities.Shared;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IDataAcessContext<TEntity> where TEntity : class, IBaseEntity
    {
        public abstract Task InsertAsync(TEntity entity);
        public abstract Task UpdateAsync(TEntity entity);
        public abstract Task DeleteAsync(TEntity entity);

        public abstract Task<TEntity?> SelectByIdAsync(string id);

        public abstract Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);

        public abstract Task<PagedResult<TEntity>> SelectPagedAsync(Query<TEntity> query);

        public abstract Task<long> CountAsync(Expression<Func<TEntity, bool>>? filter = null);

        public abstract Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
    }
}
