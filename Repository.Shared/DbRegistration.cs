using Microsoft.Extensions.DependencyInjection;

namespace Repository.Shared
{
    public static class DbRegistration
    {
        public static void RegisterEntities(this IServiceCollection services, BaseDataAccessContext<TEntity>, params Type[] entityTypes)
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
