using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace BuildingBlocks.Logging.Serilog;

public static class SerilogExtensions
{
    public static void ConfigureBootstrapLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();
    }

    public static WebApplicationBuilder AddCustomSerilog(this WebApplicationBuilder builder, string serviceName)
    {
        builder.Host.UseSerilog((context, services, loggerConfiguration) =>
        {
            var elasticUri = context.Configuration["ElasticConfiguration:Uri"];
            var dataset = serviceName.ToLowerInvariant().Replace('.', '-');

            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("service.name", dataset)
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Properties:service.name}] {Message:lj}{NewLine}{Exception}");

            if (!string.IsNullOrWhiteSpace(elasticUri))
            {
                loggerConfiguration.WriteTo.Elasticsearch(
                    [new Uri(elasticUri)],
                    opts =>
                    {
                        opts.DataStream = new DataStreamName("logs", dataset, "default");
                        opts.BootstrapMethod = BootstrapMethod.Silent;
                    });
            }
        });

        return builder;
    }

    public static WebApplication UseCustomSerilogRequestLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        return app;
    }

    public static void LogFatal(Exception ex, string message)
    {
        Log.Fatal(ex, message);
    }

    public static void CloseAndFlush()
    {
        Log.CloseAndFlush();
    }
}
