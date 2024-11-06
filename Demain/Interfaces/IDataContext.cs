using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IDataContext<TEntity> 
{
    IQueryable<TEntity> Query { get; }              
    Task InsertAsync(TEntity entity);             
    Task UpdateAsync(TEntity entity);            
    Task DeleteAsync(TEntity entity);             
    Task<TEntity?> GetByIdAsync(string id);         
    Task<List<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> filter); 
}
