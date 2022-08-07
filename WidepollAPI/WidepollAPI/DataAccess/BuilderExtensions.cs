namespace WidepollAPI.DataAccess
{
    public static class BuilderExtensions
    {
        public static IServiceCollection ConfigureRepositories(IServiceCollection services)
        {
            services.AddSingleton<IDBWriter, MongoDB>();
            services.AddSingleton<IDBReader, MongoDB>();

            return services;
        }
    }
}
