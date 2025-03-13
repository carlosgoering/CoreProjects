using Domain.Entities;

namespace Repository.Context.Providers.SQLite.Interfaces;

internal interface IDataAccessContextFactory
{
    DataAccessContext<TEntity> Create<TEntity>(string collectionName) where TEntity : IBaseEntity;
}
