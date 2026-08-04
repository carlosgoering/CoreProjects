using DataAccess.Abstractions.Interfaces;
using DataAccess.Core.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Core.Registration;

public static class DataAccessRegistration
{
    public static IServiceCollection AddDataAccessCore(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(DataRepository<>));
        return services;
    }
}