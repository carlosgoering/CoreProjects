using Domain.Entities;
using Domain.Entities.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Repository.MongoDB.Mappers;
namespace Repository.MongoDB
{
    public static class MongoDbRegistration
    {
        public static void RegisterEntities(this IServiceCollection services, params Type[] entityTypes)
        {
            
        // BsonClassMap.RegisterClassMap<BaseEntity>(cm =>
            //     {
            //         cm.AutoMap();
            //         cm.MapIdProperty(c => c.Id)
            //         .SetIdGenerator(StringObjectIdGenerator.Instance)
            //         .SetSerializer(new StringSerializer(BsonType.ObjectId));
            //     });

            foreach (var type in entityTypes)
            {
                var contextType = typeof(DataAccessContext<>).MakeGenericType(type);
                var repositoryType = typeof(DataRepository<>).MakeGenericType(type);

                services.AddSingleton(repositoryType, provider =>
                {
                    var options = provider.GetRequiredService<IOptions<Database>>();
                    var contextInstance = Activator.CreateInstance(contextType, options, type.Name);
                    return Activator.CreateInstance(repositoryType, contextInstance);
                });

            }
        }
    }
}
