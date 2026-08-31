using BuildingBlocks.Logging.Serilog;
using Microsoft.AspNetCore.RateLimiting;

SerilogExtensions.ConfigureBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddCustomSerilog("ApiGateway");

    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

    builder.Services.AddRateLimiter(ratelimiterOptions =>
    {
        ratelimiterOptions.AddFixedWindowLimiter("fixed", options =>
        {
            options.Window = TimeSpan.FromSeconds(10);
            options.PermitLimit = 5;
        });
    });

    var app = builder.Build();

    app.UseCustomSerilogRequestLogging();

    app.UseRateLimiter();
    app.MapReverseProxy();

    app.Run();
}
catch (Exception ex)
{
    SerilogExtensions.LogFatal(ex, "Application terminated unexpectedly");
}
finally
{
    SerilogExtensions.CloseAndFlush();
}
