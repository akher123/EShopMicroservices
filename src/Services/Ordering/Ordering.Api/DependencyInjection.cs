namespace Ordering.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        //services.AddCartar();
        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
      //  app.MapCartar();
        return app;
    }

}
