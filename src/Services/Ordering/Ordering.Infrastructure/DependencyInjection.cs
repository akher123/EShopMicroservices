using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        // Add services to the Container

        //services.AddDbContext<ApplicationDbContext>(options => options.UserSqlServer(connectionString));

      //  services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}
