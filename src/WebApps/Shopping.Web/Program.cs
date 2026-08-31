using BuildingBlocks.Logging.Serilog;
using BuildingBlocks.Resilience;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

SerilogExtensions.ConfigureBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddCustomSerilog("Shopping.Web");

    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddRefitClient<ICatalogService>()
        .ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
        })
        .AddDefaultResilienceHandler(builder.Configuration, "CatalogService");
    builder.Services.AddRefitClient<IBasketService>()
        .ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
        })
        .AddDefaultResilienceHandler(builder.Configuration, "BasketService");
    builder.Services.AddRefitClient<IOrderingService>()
        .ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
        })
        .AddDefaultResilienceHandler(builder.Configuration, "OrderingService");

    var gatewayAddress = builder.Configuration["ApiSettings:GatewayAddress"]!;
    builder.Services.AddHealthChecks()
        .AddCheck("self", () => HealthCheckResult.Healthy())
        .AddUrlGroup(
            new Uri($"{gatewayAddress.TrimEnd('/')}/health"),
            name: "api-gateway",
            configurePrimaryHttpMessageHandler: builder.Environment.IsDevelopment()
                ? _ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }
                : null);

    var app = builder.Build();

    app.UseCustomSerilogRequestLogging();

    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseWhen(
        context => !context.Request.Path.StartsWithSegments("/health"),
        appBuilder => appBuilder.UseHttpsRedirection());

    app.UseRouting();

    app.UseAuthorization();

    app.MapStaticAssets();
    app.MapRazorPages()
       .WithStaticAssets();

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
