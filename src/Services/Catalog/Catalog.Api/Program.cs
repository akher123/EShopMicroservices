using BuildingBlocks.Logging.Serilog;
using Catalog.Api.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

SerilogExtensions.ConfigureBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddCustomSerilog("Catalog.Api");

    var assembly = typeof(Program).Assembly;

    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssemblies(assembly);
        config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    });

    builder.Services.AddValidatorsFromAssembly(assembly);

    builder.Services.AddCarter();

    builder.Services.AddMarten(opts =>
    {
        opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    }).UseLightweightSessions();

    if (builder.Environment.IsDevelopment())
    {
        builder.Services.InitializeMartenWith<CatalogInitialData>();
    }

    builder.Services.AddExceptionHandler<CustomExceptionHandler>();

    builder.Services.AddHealthChecks()
           .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

    var app = builder.Build();

    app.UseCustomSerilogRequestLogging();

    app.MapCarter();

    app.UseExceptionHandler(opts => { });

    app.UseHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

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
