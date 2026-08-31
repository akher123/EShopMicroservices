using BuildingBlocks.Logging.Serilog;

SerilogExtensions.ConfigureBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddCustomSerilog("WebStatus");

    builder.Services
        .AddHealthChecksUI()
        .AddInMemoryStorage();

    var app = builder.Build();

    app.UseCustomSerilogRequestLogging();

    app.MapHealthChecksUI(options => options.UIPath = "/hc-ui");

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
