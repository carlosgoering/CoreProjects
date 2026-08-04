using DataAccess.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace DataAccess.Core.Registration;

public static class DataAccessRegistration
{
    public static IServiceCollection AddDataAccessCore(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(DataRepository<>));
        return services;
    }
}