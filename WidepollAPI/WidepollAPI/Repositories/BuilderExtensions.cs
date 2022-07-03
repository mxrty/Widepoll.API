namespace WidepollAPI.Repositories
{
    public static class BuilderExtensions
    {
        public static IServiceCollection ConfigureRepositories(IServiceCollection services)
        {
            services.AddSingleton<IStatementRepository, StatementRepository>();
            services.AddSingleton<ILikeRepository, LikeRepository>();

            return services;
        }
    }
}
