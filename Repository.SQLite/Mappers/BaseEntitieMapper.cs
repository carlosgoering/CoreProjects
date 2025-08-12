using Domain.Entities;
using Domain.Interfaces;

namespace Repository.SQLite.Mappers;
public class EntityMapper : IEntityMapper<BaseEntity, Entity>
{
    public Entity ToPersisted(BaseEntity domain)
    {
        return new Entity
        {
            id = domain.id,
            externalIdentity = domain.externalIdentity
        };
    }

    public BaseEntity ToDomain(Entity persisted)
    {
        return new BaseEntity
        {
            id = persisted.id,
            externalIdentity = persisted.externalIdentity
        };
    }
}