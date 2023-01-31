using MongoDB.Driver;
using MongoDB.Entities;

namespace WidepollAPI.DataAccess;

public static class BuilderExtensions
{
    public static IServiceCollection ConfigureDataAccess(IServiceCollection services, string connectionString)
    {
        Task.Run(async () =>
        {
            await DB.InitAsync("dev", MongoClientSettings.FromConnectionString(connectionString));
        }).GetAwaiter().GetResult();
        services.AddSingleton<IDBWriter, MongoStore>();
        services.AddSingleton<IDBReader, MongoStore>();

        return services;
    }
}
