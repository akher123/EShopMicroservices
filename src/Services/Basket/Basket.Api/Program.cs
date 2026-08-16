using Basket.Api.Data;
using Basket.Api.Exceptions.Handler;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container


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
builder.Services.AddHealthChecks()
       .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();


// Configure the HTTP request pipeline
app.MapCarter();

app.UseExceptionHandler(opts => { });

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
