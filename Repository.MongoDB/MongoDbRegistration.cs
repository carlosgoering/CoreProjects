using Domain.Entities;
using Domain.Entities.Configuration;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Repository.MongoDB.Mappers;

namespace Repository.MongoDB
{
    public static class MongoDbRegistration
    {
        public static void RegisterEntities(this IServiceCollection services, params Type[] entityTypes)
        {
            services.AddSingleton<IEntityMapper<BaseEntity, Entity>, EntityMapper>();

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
