using Domain.Entities;

namespace Repository.MongoDB.Mappers;
public class EntityMapper : IEntityMapper<BaseEntity, Entity>
{
    public MongoUser ToPersisted(BaseEntity domain)
    {
        return new Entity
        {
            Id = domain.Id,
            Name = domain.Name,
            ExternalIdentity = domain.ExternalIdentity
        };
    }

    public User ToDomain(Entity persisted)
    {
        return new BaseEntity
        {
            Id = persisted.Id,
            Name = persisted.Name,
            ExternalIdentity = persisted.ExternalIdentity
        };
    }
}