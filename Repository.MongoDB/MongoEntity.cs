using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Repository.MongoDB;
public class MongoEntity : BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string MongoId
    {
        get => Id;
        set => Id = value;
    }
}