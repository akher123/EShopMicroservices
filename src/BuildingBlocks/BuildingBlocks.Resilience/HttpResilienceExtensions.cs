using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Resilience;

public static class HttpResilienceExtensions
{
    public static IHttpClientBuilder AddDefaultResilienceHandler(
        this IHttpClientBuilder builder,
        IConfiguration configuration,
        string clientName)
    {
        var section = configuration.GetSection($"Resilience:{clientName}");

        builder.AddStandardResilienceHandler(options =>
        {
            options.Retry.MaxRetryAttempts = section.GetValue("MaxRetryAttempts", 3);
            options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(
                section.GetValue("TimeoutSeconds", 30));
            options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(
                section.GetValue("CircuitBreakerSamplingSeconds", 30));
        });

        return builder;
    }
}
