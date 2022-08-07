using MongoDB.Driver;
using MongoDB.Entities;

namespace WidepollAPI.DataAccess;

public static class BuilderExtensions
{
    public static IServiceCollection ConfigureDataAccess(IServiceCollection services)
    {
        Task.Run(async () =>
        {
            await DB.InitAsync("testDB",
            MongoClientSettings.FromConnectionString(
            "mongodb+srv://admin:deUsKxSO8O2zBxSr@cluster0.j8fil.mongodb.net/?retryWrites=true&w=majority"));
        }).GetAwaiter().GetResult();
        services.AddSingleton<IDBWriter, MongoDB>();
        services.AddSingleton<IDBReader, MongoDB>();

        return services;
    }
}
