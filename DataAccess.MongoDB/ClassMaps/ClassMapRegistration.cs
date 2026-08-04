using DataAccess.Abstractions.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace DataAccess.MongoDB.ClassMaps
{
    internal static class MongoClassMapRegistration
    {
        private static bool initialized;

        public static void Register()
        {
            if (initialized)
                return;

            initialized = true;

            BsonClassMap.RegisterClassMap<IBaseEntity>(cm =>
            {
                cm.AutoMap();

                cm.MapIdProperty(x => x.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });
        }
    }
}
