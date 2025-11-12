using Domain.Entities;
using Domain.Entities.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
namespace Repository.MongoDB
{
    public static class MongoDbRegistration
    {
        public static void RegisterEntities(this IServiceCollection services, params Type[] entityTypes)
        {
            BsonClassMap.RegisterClassMap<IBaseEntity>(cm =>
                    {
                        cm.AutoMap();
                        cm.MapIdProperty(c => c.Id)
                        .SetIdGenerator(StringObjectIdGenerator.Instance)
                        .SetSerializer(new StringSerializer(BsonType.ObjectId));
                    });

            foreach (var type in entityTypes)
            {
                var contextType = typeof(DataAccessContext<>).MakeGenericType(type);
                var repositoryType = typeof(DataRepository<>).MakeGenericType(type);

                services.AddSingleton(repositoryType, provider =>
                {
                    var options = provider.GetRequiredService<IOptions<IDatabase>>();
                    var contextInstance = Activator.CreateInstance(contextType, options, type.Name);
                    var instance = Activator.CreateInstance(repositoryType, contextInstance);

                    return instance ?? throw new Exception($"Could not create instance of type {repositoryType.FullName}");
                });

            }
        }
    }
}
