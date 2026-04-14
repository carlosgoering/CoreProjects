using Domain.Entities.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Repository.SQLite
{
    public static class SQLiteDbRegistration
    {
        public static void RegisterEntities(this IServiceCollection services, params Type[] entityTypes)
        {
            foreach (var type in entityTypes)
            {
                var contextType = typeof(DataAccessContext<>).MakeGenericType(type);
                var repositoryType = typeof(DataRepository<>).MakeGenericType(type);

                services.AddSingleton(repositoryType, provider =>
                {
                    var options = provider.GetRequiredService<IOptions<Database>>();
                    var contextInstance = Activator.CreateInstance(contextType, options);
                    var instance = Activator.CreateInstance(repositoryType, contextInstance);

                    return instance is null ? throw new Exception($"Could not create instance of type {repositoryType.FullName}") : instance;
                });
            }
        }
    }
}
