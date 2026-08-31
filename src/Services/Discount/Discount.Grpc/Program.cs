using BuildingBlocks.Logging.Serilog;
using Discount.Grpc.Data;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;

SerilogExtensions.ConfigureBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddCustomSerilog("Discount.Grpc");

    builder.Services.AddGrpc();

    builder.Services.AddDbContext<DicountContext>(opts =>
        opts.UseSqlite(builder.Configuration.GetConnectionString("Database")));

    var app = builder.Build();

    app.UseCustomSerilogRequestLogging();

    app.UseMigration();

    app.MapGrpcService<DiscountService>();

    app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

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
