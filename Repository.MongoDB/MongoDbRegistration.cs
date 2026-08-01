using Domain.Entities.Configuration;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
namespace Repository.MongoDB
{
    public static class MongoDbRegistration
    {
 public static class MongoRegistration
{
    public static DataAccessOptions UseMongo(
        this DataAccessOptions options,
        IConfigurationSection configuration)
    {
        options.Services.Configure<Database>(configuration);

        options.Services.AddSingleton<IMongoClient>(sp =>
        {
            var database = sp.GetRequiredService<IOptions<Database>>().Value;

            return new MongoClient(database.ConnectionString);
        });

        options.Services.AddSingleton<IMongoDatabase>(sp =>
        {
            var database = sp.GetRequiredService<IOptions<Database>>().Value;

            return sp.GetRequiredService<IMongoClient>()
                     .GetDatabase(database.DatabaseName);
        });

        options.Services.AddSingleton(typeof(IDataAcessContext<>), typeof(DataAccessContext<>));

        return options;
    }
}
