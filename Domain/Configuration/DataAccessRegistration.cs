using Domain.Interfaces;

namespace Domain.Entities.Configuration;

public static class DataAccessRegistration
{
    public static IServiceCollection AddDataAccess(
        this IServiceCollection services,
        Action<DataAccessOptions> configure)
    {
        var options = new DataAccessOptions(services);

        configure(options);

        services.AddScoped(typeof(IRepository<>), typeof(DataRepository<>));

        return services;
    }
}