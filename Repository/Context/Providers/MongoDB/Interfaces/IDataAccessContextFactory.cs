using Domain.Entities;

namespace Repository.Context.Providers.MongoDB.Interfaces;

internal interface IDataAccessContextFactory
{
    DataAccessContext<TEntity> Create<TEntity>(string collectionName) where TEntity : IBaseEntity;
}
