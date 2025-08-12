using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Repository.MongoDB;

public class Entity : BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public new string id { get; set; }
}