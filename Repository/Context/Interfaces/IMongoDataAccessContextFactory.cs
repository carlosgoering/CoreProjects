using Domain.Entities;

namespace Repository.Context.Interfaces;

public interface IMongoDataAccessContextFactory
{
    MongoDataAccessContext<TEntity> Create<TEntity>(string collectionName) where TEntity : IBaseEntity;
}
