using WidepollAPI.Controllers.Translators;

namespace WidepollAPI.Controllers;

public static class BuilderExtensions
{
    public static IServiceCollection ConfigureControllers(IServiceCollection services)
    {
        return services.AddSingleton<IUserTranslator, UserTranslator>();
    }
}
