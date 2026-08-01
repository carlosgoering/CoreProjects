using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.SQLite
{
    public static class SQLiteDbRegistration
    {
        public static void RegisterEntities(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IDataAcessContext<>), typeof(DataAccessContext<>));
            services.AddScoped(typeof(IRepository<>), typeof(DataRepository<>));
        }
    }
}
