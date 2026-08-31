using Basket.Api.Data;
using Basket.Api.Exceptions.Handler;
using BuildingBlocks.Logging.Serilog;
using BuildingBlocks.Messaging.MassTransit;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Reflection;

SerilogExtensions.ConfigureBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddCustomSerilog("Basket.Api");

    var assyembly = typeof(Program).Assembly;
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(assyembly);
        config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    });

    builder.Services.AddCarter();
    builder.Services.AddMarten(opts =>
    {
        opts.Connection(builder.Configuration.GetConnectionString("Database")!);
        opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);

    }).UseLightweightSessions();

    builder.Services.AddExceptionHandler<CustomExceptionHandler>();

    builder.Services.AddScoped<IBasketRepository, BasketRepository>();

    builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis");
    });

    builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
    {
        options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
    })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            return handler;
        });

    builder.Services.AddMesageBroker(builder.Configuration);

    builder.Services.AddHealthChecks()
           .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
           .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

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
