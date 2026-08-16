var builder = WebApplication.CreateBuilder(args);
// Add services to the container

builder.Services.AddCarter();

var assyembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assyembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});


var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();

app.Run();
