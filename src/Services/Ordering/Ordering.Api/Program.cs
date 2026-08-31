using BuildingBlocks.Logging.Serilog;
using Ordering.Api;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

SerilogExtensions.ConfigureBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddCustomSerilog("Ordering.Api");

    builder.Services
        .AddAppllicationServices(builder.Configuration)
        .AddInfrastructureServices(builder.Configuration)
        .AddApiServices(builder.Configuration);

    var app = builder.Build();

    app.UseCustomSerilogRequestLogging();

    app.UseApiServices();

    if (app.Environment.IsDevelopment())
    {
        await app.Services.InitialiseDatabaseAsync();
    }

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
