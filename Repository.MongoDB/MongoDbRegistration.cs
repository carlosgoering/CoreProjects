using Domain.Entities;
using Domain.Entities.Configuration;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
namespace Repository.MongoDB
{
    public static class MongoDbRegistration
    {
        public static void RegisterEntities(this IServiceCollection services)
        {
            BsonClassMap.RegisterClassMap<IBaseEntity>(cm =>
                    {
                        cm.AutoMap();
                        cm.MapIdProperty(c => c.Id)
                        .SetIdGenerator(StringObjectIdGenerator.Instance)
                        .SetSerializer(new StringSerializer(BsonType.ObjectId));
                    });

            services.AddSingleton(typeof(IDataAcessContext<>), typeof(DataAccessContext<>));
            services.AddScoped(typeof(IRepository<>), typeof(DataRepository<>));
        }
    }
}
