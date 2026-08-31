using BuildingBlocks.Logging.Serilog;
using BuildingBlocks.Resilience;

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

    var app = builder.Build();

    app.UseCustomSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();

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
