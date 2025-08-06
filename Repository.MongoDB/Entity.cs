using Domain.Entities;

namespace Repository.MongoDB;

public class Entity : BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public new string Id { get; set; }
}